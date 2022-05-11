using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PeliculasWebAPI.Entidades.Configuraciones {
    public class CineConfig : IEntityTypeConfiguration<Cine> {
        public void Configure(EntityTypeBuilder<Cine> builder) {
            /* Propiedades de la Tabla Cine */
            builder.Property(prop => prop.Nombre)
                   .HasMaxLength(150)
                   .IsRequired();

        }
    }
}
