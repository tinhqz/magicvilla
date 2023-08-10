using System.ComponentModel.DataAnnotations;

namespace MagicVillaApi.Modelos.Dto
{
    public class VillaCreateDto
    {
        [Required] // validaciones DataAnnotations
        [MaxLength(30)]
        public string Nombre { get; set; }
        [Required]
        public string Detalle { get; set; }
        [Required]
        public double Tarifa { get; set; }

        public int Ocupantes { get; set; }

        public int MetrosCuadrados { get; set; }

        public string ImageUrl { get; set; }

        public string Amenidad { get; set; }
    }
}
