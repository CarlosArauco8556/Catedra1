using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catedra1.src.Models;

namespace Catedra1.src.Interfaces
{
    public interface IUserRepository
    {
        List<User> Get(string? sort = null, string? gender = null);
        User Post(User user);
        User? Put(int id, User user);
        User? Delete(int id);
    }
}