using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PeliculasWebAPI.Entidades.Configuraciones {
    public class PeliculaConfig : IEntityTypeConfiguration<Pelicula> {
        public void Configure(EntityTypeBuilder<Pelicula> builder) {
            /* Propiedades de la Tabla Pelicula */
            builder.Property(prop => prop.Titulo)
                   .HasMaxLength(250)
                   .IsRequired();

            builder.Property(prop => prop.PosterURL)
                   .HasMaxLength(500)
                   .IsUnicode(false);
        }
    }
}
