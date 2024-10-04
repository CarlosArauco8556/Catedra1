using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catedra1.src.Models;

namespace Catedra1.src.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> Get();
        Task<User> Post(User user);
        Task<User?> Put(int id, User user);
        Task<User?> Delete(int id);
    }
}