using Cards.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Domain.IRepository;

namespace Cards.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly CardsDbContext _context;

        public UserRepository(CardsDbContext context)
        {
            _context = context;
        }
        public async Task<User> AddUser(User user)
        {
            user.CreatedAt = DateTime.Now;
            user.CreatedBy = user.Id;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> Delete(Guid Id)
        {
            var user = await _context.Users.FindAsync(Id);

            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<User>> GetAllUsersAdmin()
        {
            return await _context.Users.OrderBy(p => p.CreatedAt).ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAllUsersMember(Guid Id)
        {
            return await _context.Users.Where(p => p.CreatedBy == Id).OrderBy(p => p.CreatedAt).ToListAsync();
        }

        public async Task<User> Update(User user, Guid ModifiedById)
        {
            user.ModifiedAt = DateTime.Now;
            user.ModifiedBy = ModifiedById;
            _context.Entry(user).State = EntityState.Modified;
            var x = await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetUserById(Guid Id)
        {
            return await _context.Users.Where(p => p.Id == Id).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByUserEmailAndPassword(string Email, string Password)
        {
            return await _context.Users.Where(p => p.Email == Email && p.Password == Password).FirstOrDefaultAsync();
        }
    }
}
