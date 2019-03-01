using Microsoft.EntityFrameworkCore;
using netcore.infrastructure.Entities;
using netcore.infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace netcore.infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        public SampleDbContext _context { get; set; }
        public UserRepository(SampleDbContext sampleDbContext)
        {
            _context = sampleDbContext;
        }

        async public Task<IEnumerable<User>> GetAll()
        {
            return await _context.User.ToListAsync();
        }

        async public Task<User> GetById(Guid id)
        {
            var user = await _context.User.FindAsync(id);
            return user;
        }

        async public Task Remove(Guid id)
        {
            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
        }

        async public Task<User> Add(User user)
        {
            _context.User.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        async public Task<User> Update(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return user;
        }

        private bool UserExists(Guid id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
