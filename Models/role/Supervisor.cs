using System.ComponentModel.DataAnnotations.Schema;

namespace Peajes.Models.role
    {
    public class Empleado
    {
        Persona persona = new Persona();
        public int id { get; set; }
        [ForeignKey("Persona")]
        public int PersonaId { get; set; }
        public string Puesto { get; set; }
        public int AñosDeTrabajo { get; set; }
        }
    }
