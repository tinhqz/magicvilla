using System.ComponentModel.DataAnnotations;

namespace MagicVillaApi.Modelos.Dto
{
    public class VillaDto
    {
        public int Id { get; set; }
        [Required] // validaciones DataAnnotations
        [MaxLength(30)]
        public string Nombre { get; set; }

        public string Detalle { get; set; }
        [Required]
        public double Tarifa { get; set; }

        public int Ocupantes { get; set; }

        public int MetrosCuadrados { get; set; }

        public string ImageUrl { get; set; }

        public string Amenidad { get; set; }
    }
}
