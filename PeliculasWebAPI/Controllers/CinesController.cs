using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using PeliculasWebAPI.DTOs;
using PeliculasWebAPI.Entidades;
using PeliculasWebAPI.Entidades.SinLlaves;

namespace PeliculasWebAPI.Controllers {
    [ApiController]
    [Route("api/cines")]
    public class CinesController : ControllerBase {
        private readonly ApplicationDBContext context;
        private readonly IMapper mapper;

        public CinesController(ApplicationDBContext context, IMapper mapper) {
            this.context = context;
            this.mapper = mapper;
        }

        public object NtsGeometryService { get; private set; }

        [HttpGet]
        public async Task<IEnumerable<CineDTO>> Get() {
            return await context.Cines
                                .ProjectTo<CineDTO>(mapper.ConfigurationProvider)
                                .ToListAsync();
        }

        [HttpGet("cercanos")]
        public async Task<ActionResult> Get(double lat, double lng) {
            var geoFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
            var miUbicacion = geoFactory.CreatePoint(new Coordinate(lat, lng));
            var distanciaMax = 2000;

            var cines = await context.Cines
                                     .OrderBy(c => c.Ubicacion.Distance(miUbicacion))
                                     .Where(c => c.Ubicacion.IsWithinDistance(miUbicacion, distanciaMax))
                                     .Select(c => new {
                                         Nombre = c.Nombre,
                                         Distancia = Math.Round(c.Ubicacion.Distance(miUbicacion))
                                     })
                                     .ToListAsync();

            return Ok(cines);
        }

        [HttpPost]
        public async Task<ActionResult> Post() {
            var geoFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
            var ubicacionCine = geoFactory.CreatePoint(new Coordinate(-69.896979, 18.476276));

            var cine = new Cine() {
                Nombre = "Cinemex con Mon",
                Ubicacion = ubicacionCine,
                CineOferta = new CineOferta() {
                    DescuentoPorcentaje = 5,
                    FechaInicio = DateTime.Today,
                    FechaFin = DateTime.Today.AddDays(7)
                },
                SalaCine = new HashSet<SalaCine>() {
                    new SalaCine() {
                        Precio       = 200,
                        Moneda       = Moneda.PesoMex,
                        TipoSalaCine = TipoSalaCine.DosD
                    },
                    new SalaCine() {
                        Precio       = 350,
                        Moneda       = Moneda.DolarEUA,
                        TipoSalaCine = TipoSalaCine.TresD
                    }
                }
            };

            context.Add(cine);
            await context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("conDTO")]
        public async Task<ActionResult> PostConDTO(CineCreacionDTO cineCreacionDTO) {
            var cine = mapper.Map<Cine>(cineCreacionDTO);
            context.Add(cine);

            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("sinUbicacion")]
        public async Task<IEnumerable<CineSinUbicacion>> GetCinesUbicacion(){
            /* Propiedad set permite crear un DbContext en tiempo real */
            // return await context.Set<CineSinUbicacion>().ToListAsync();
            return await context.CineSinUbicacion.ToListAsync();
        }
    }
}
