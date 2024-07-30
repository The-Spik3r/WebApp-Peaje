using System.ComponentModel.DataAnnotations;

namespace Peajes.Models.entity
{
    public class Colectivo 
    {
        public int id { get; set; }
        [Required]
        public string matricula { get; set; }
        [Required]
        public string marca { get; set; }
        [Required]
        public string modelo { get; set; }
        [Required]
        public int pisos { get; set; }
    }
}
