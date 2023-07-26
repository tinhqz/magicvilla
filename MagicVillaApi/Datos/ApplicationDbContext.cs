using MagicVillaApi.Modelos;
using Microsoft.EntityFrameworkCore;

namespace MagicVillaApi.Datos
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<Villa> Villas { get; set; }
    }
}
