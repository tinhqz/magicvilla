using System.ComponentModel.DataAnnotations;

namespace MagicVillaApi.Modelos.Dto
{
    public class VillaUpdateDto
    {
        [Required]
        public int Id { get; set; }
        [Required] // validaciones DataAnnotations
        [MaxLength(60)]
        public string Nombre { get; set; }
        public string Detalle { get; set; }
        [Required]
        public double Tarifa { get; set; }

        // Solo habilitar la actualizacion de los primeros campos
        public int Ocupantes { get; set; }

        public int MetrosCuadrados { get; set; }

        public string ImageUrl { get; set; }

        public string Amenidad { get; set; }
    }
}
