using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.DAL.Entities;

namespace Agora.DAL.Interfaces
{
    public interface IUserRepository
    {
        Task<IQueryable<User>> GetAll();
        Task<User> GetByEmail(string email);
        Task<User> Get(int id);
        Task Create(User user);  
        void Update(User user);
        Task Delete(int id);

       
    }
}
