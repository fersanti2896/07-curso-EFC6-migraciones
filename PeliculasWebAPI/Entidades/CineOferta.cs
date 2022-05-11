namespace PeliculasWebAPI.Entidades {
    public class CineOferta {
        public int Id { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal DescuentoPorcentaje { get; set; }
        public int CineId { get; set; }
    }
}
