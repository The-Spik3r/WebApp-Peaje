using System.ComponentModel.DataAnnotations.Schema;

namespace Peajes.Models.role
    {
    public class Supervisor
    {
        Persona persona = new Persona();
        public int id { get; set; }
        [ForeignKey("Persona")]
        public int PersonaId { get; set; }
        public int AñosDeTrabajo { get; set; }
        }
    }
