using Cards.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Domain.IRepository
{
    public interface IUserRepository
    {
        Task<User> AddUser(User user);
        Task<User> Update(User user, Guid ModifiedById);
        Task<bool> Delete(Guid Id);
        Task<IEnumerable<User>> GetAllUsersAdmin();
        Task<IEnumerable<User>> GetAllUsersMember(Guid Id);
        Task<User> GetUserById(Guid Id);
        Task<User> GetUserByUserEmailAndPassword(string Email, string Password);
    }
}
