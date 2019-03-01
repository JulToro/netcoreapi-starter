using netcore.infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace netcore.infrastructure.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAll();
        Task<User> GetById(Guid id);
        Task Remove(Guid id);
        Task<User> Add(User p);
        Task<User> Update(User p);
    }
}
