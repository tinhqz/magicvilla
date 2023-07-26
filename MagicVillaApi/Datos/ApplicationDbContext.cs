using MagicVillaApi.Modelos;
using Microsoft.EntityFrameworkCore;

namespace MagicVillaApi.Datos
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Villa> Villas { get; set; }

        // comandos para migraciones
        // $ add-migration agregarBaseDatos
        // $ update-database
    }
}
