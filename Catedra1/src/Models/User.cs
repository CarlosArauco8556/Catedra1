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

        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe ser mayor a 3 y menor que 100 caracteres.")]
        public string Name { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
        public string CorreoElectronico { get; set; } = string.Empty;

        [RegularExpression(@"masculino|femenino|otro|prefiero no decirlo", ErrorMessage = "El género debe ser “masculino”, “femenino”, “otro” o “prefiero no decirlo”.")]
        public string Genero { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateOnly FechaNacimiento { get; set; }
    }
}