using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catedra1.src.Data;
using Catedra1.src.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catedra1.src.Controllers
{
    
    [Route("Catedra1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public UserController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get(string? sort = null, string? gender = null)
        {
            var usersQuery = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(gender))
            {
                var validGenders = new[] { "masculino", "femenino", "otro", "prefiero no decirlo" };
                if (!validGenders.Contains(gender.ToLower()))
                {
                    return BadRequest(new { Message = "Filtro de género inválido." });
                }

                usersQuery = usersQuery.Where(u => u.Genero.ToLower() == gender.ToLower());
            }

            if (!string.IsNullOrEmpty(sort))
            {
                if (sort.ToLower() == "asc")
                {
                    usersQuery = usersQuery.OrderBy(u => u.Name);
                }
                else if (sort.ToLower() == "desc")
                {
                    usersQuery = usersQuery.OrderByDescending(u => u.Name);
                }
                else
                {
                    return BadRequest(new { Message = "Valor de ordenación inválido. Usar 'asc' o 'desc'." });
                }
            }

            var users = usersQuery.ToList();

            return Ok(users);
        }
        
        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Rut == user.Rut);
            if (existingUser != null)
            {
                return Conflict(new { Message = "El RUT ya existe." });
            }

            if (user.FechaNacimiento >= DateOnly.FromDateTime(DateTime.Now))
            {
                return BadRequest(new { Message = "La fecha de nacimiento debe ser menor a la fecha actual." });
            }

            _context.Users.Add(user);
            _context.SaveChanges();

            return CreatedAtAction(nameof(Post), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromRoute] int id, [FromBody] User user)
        {
            var userToUpdate = _context.Users.FirstOrDefault(u => u.Id == id);
            if (userToUpdate == null)
            {
                return NotFound(new { Message = "Usuario no encontrado." });
            }

            if (user.Rut != userToUpdate.Rut && _context.Users.Any(u => u.Rut == user.Rut))
            {
                return BadRequest(new { Message = "El RUT ya existe." });
            }

            if (user.FechaNacimiento >= DateOnly.FromDateTime(DateTime.Now))
            {
                return BadRequest(new { Message = "La fecha de nacimiento debe ser menor a la fecha actual." });
            }
            
            userToUpdate.Rut = user.Rut;
            userToUpdate.Name = user.Name;
            userToUpdate.CorreoElectronico = user.CorreoElectronico;
            userToUpdate.Genero = user.Genero;
            userToUpdate.FechaNacimiento = user.FechaNacimiento;
            _context.SaveChanges();
            return Ok(userToUpdate);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound(new{ Message = "Usuario no encontrado."});
            }
            _context.Users.Remove(user);
            _context.SaveChanges();
            return Ok("Usario eliminado exitosamente");
        }

    }
}