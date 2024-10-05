using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catedra1.src.Data;
using Catedra1.src.Interfaces;
using Catedra1.src.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catedra1.src.Controllers
{
    
    [Route("Catedra1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult Get(string? sort = null, string? gender = null)
        {
            try
            {
                var users = _userRepository.Get(sort, gender);
                return Ok(users);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
        
        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {
            try
            {
                var createdUser = _userRepository.Post(user);
                return CreatedAtAction(nameof(Post), new { id = createdUser.Id }, createdUser);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromRoute] int id, [FromBody] User user)
        {
            try
            {
                var userToUpdate = _userRepository.Put(id, user);
                if (userToUpdate == null)
                {
                    return NotFound(new { Message = "Usuario no encontrado." });
                }

                return Ok(userToUpdate);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
        var user = _userRepository.Delete(id);
        if (user == null)
        {
            return NotFound(new { Message = "Usuario no encontrado." });
        }
        return Ok("Usuario eliminado exitosamente");
        }

    }
}