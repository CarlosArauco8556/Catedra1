using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catedra1.src.Data;
using Catedra1.src.Interfaces;
using Catedra1.src.Models;

namespace Catedra1.src.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDBContext _context;
        public UserRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public User? Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return null;
            }

            _context.Users.Remove(user);
            _context.SaveChanges();
            return user;
        }

        public List<User> Get(string? sort = null, string? gender = null)
        {
            var usersQuery = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(gender))
            {
                var validGenders = new[] { "masculino", "femenino", "otro", "prefiero no decirlo" };
                if (!validGenders.Contains(gender.ToLower()))
                {
                    throw new ArgumentException("Filtro de género inválido."); 
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
                    throw new ArgumentException("Valor de ordenación inválido. Usar 'asc' o 'desc'."); 
                }
            }

            return usersQuery.ToList(); 
        }

        public User Post(User user)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Rut == user.Rut);
            if (existingUser != null)
            {
                throw new InvalidOperationException("El RUT ya existe.");
            }

            if (user.FechaNacimiento >= DateOnly.FromDateTime(DateTime.Now))
            {
                throw new ArgumentException("La fecha de nacimiento debe ser menor a la fecha actual."); 
            }

            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public User? Put(int id, User user)
        {
            var userToUpdate = _context.Users.Find(id);
            if (userToUpdate == null)
            {
                return null;
            }
 
            if (user.Rut != userToUpdate.Rut && _context.Users.Any(u => u.Rut == user.Rut))
            {
                throw new InvalidOperationException("El RUT ya existe."); 
            }

            if (user.FechaNacimiento >= DateOnly.FromDateTime(DateTime.Now))
            {
                throw new ArgumentException("La fecha de nacimiento debe ser menor a la fecha actual."); 
            }

            userToUpdate.Rut = user.Rut;
            userToUpdate.Name = user.Name;
            userToUpdate.CorreoElectronico = user.CorreoElectronico;
            userToUpdate.Genero = user.Genero;
            userToUpdate.FechaNacimiento = user.FechaNacimiento;

            _context.SaveChanges();
            return userToUpdate;
        }
    }
}