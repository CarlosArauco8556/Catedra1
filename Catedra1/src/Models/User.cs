using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Catedra1.src.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "El RUT es obligatorio.")]
        public string Rut { get; set; } = string.Empty;

        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
        public string CorreoElectronico { get; set; } = string.Empty;

        [RegularExpression(@"masculino|femenino|otro|prefiero no decirlo", ErrorMessage = "Género no válido.")]
        public string Genero { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        [DataType(DataType.Date)]
        public DateOnly FechaNacimiento { get; set; }

        public bool IsFechaNacimientoValida()
        {
            return FechaNacimiento < DateOnly.FromDateTime(DateTime.Now);
        }
    }
}