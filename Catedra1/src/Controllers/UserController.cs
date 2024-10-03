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
        public IActionResult Get()
        {
            var users = _context.Users.ToList();
            return Ok(users);
        }
        
        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok(user);
        }

        [HttpPut("{rut}")]
        public IActionResult Put([FromRoute] string rut, [FromBody] User user)
        {

            var userToUpdate = _context.Users.FirstOrDefault(u => u.Rut == rut);
            if (userToUpdate == null)
            {
                return NotFound();
            }
            userToUpdate.Rut = user.Rut;
            userToUpdate.Name = user.Name;
            userToUpdate.CorreoElectronico = user.CorreoElectronico;
            userToUpdate.Genero = user.Genero;
            userToUpdate.FechaNacimiento = user.FechaNacimiento;
            _context.SaveChanges();
            return Ok(userToUpdate);
        }

        [HttpDelete("{rut}")]
        public IActionResult Delete([FromRoute] string rut)
        {
            var user = _context.Users.FirstOrDefault(u => u.Rut == rut);
            if (user == null)
            {
                return NotFound();
            }
            _context.Users.Remove(user);
            _context.SaveChanges();
            return Ok("User Deleted");
        }

    }
}