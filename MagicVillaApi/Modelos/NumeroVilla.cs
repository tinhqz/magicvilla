using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicVillaApi.Modelos
{
    public class NumeroVilla
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int VillaNo { get; set; }
        
        [Required]
        public int VillaId { get; set; }

        // Asi se relacion una tabla con otra
        [ForeignKey("VillaId")]
        public Villa Villa { get; set; }
        
        public string DetalleEspecial { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
    }
}
