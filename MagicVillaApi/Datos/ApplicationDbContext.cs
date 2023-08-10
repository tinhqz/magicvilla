using MagicVillaApi.Modelos;
using Microsoft.EntityFrameworkCore;

namespace MagicVillaApi.Datos
{
    // comandos para migraciones
    // $ add-migration agregarBaseDatos
    // $ update-database
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Villa> Villas { get; set; }
        public DbSet<NumeroVilla> NumeroVillas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa()
                {
                    Id = 1,
                    Nombre = "Villa real",
                    Detalle = "Detalle de la villa",
                    ImageUrl = "",
                    Ocupantes = 5,
                    MetrosCuadrados = 20,
                    Tarifa = 200,
                    Amenidad = "",
                    FechaCreacion = DateTime.Now,
                    FechaActualizacion = DateTime.Now
                },
                new Villa()
                {
                    Id = 2,
                    Nombre = "Premiun Vista a la Piscina",
                    Detalle = "Segundo detalle de la villa",
                    ImageUrl = "",
                    Ocupantes = 2,
                    MetrosCuadrados = 100,
                    Tarifa = 20000,
                    Amenidad = "",
                    FechaCreacion = DateTime.Now,
                    FechaActualizacion = DateTime.Now
                }

            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
