using System.ComponentModel.DataAnnotations;

namespace Peajes.Models.entity
{
    public class Moto
    {
        public int id { get; set; }
        [Required]
        public string matricula { get; set; }
        [Required]
        public int cilindraje { get; set; }
        [Required]
        public string modelo { get; set; }
        [Required]
        public string marca { get; set; }

    }
}
