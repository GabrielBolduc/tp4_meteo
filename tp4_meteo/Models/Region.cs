using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tp4_meteo.Models
{
    public class Region
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Nom { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}