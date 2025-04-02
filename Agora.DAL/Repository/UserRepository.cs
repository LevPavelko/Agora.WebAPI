using Agora.DAL.EF;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Agora.DAL.Repository
{
    public class UserRepository: IUserRepository
    {
        private AgoraContext db;
        public UserRepository(AgoraContext context)
        {
            this.db = context;
        }

        public async Task<IQueryable<User>> GetAll()
        {
            return db.Users;
        }

        public async Task<User> Get(int id)
        {
            return await db.Users.FindAsync(id);
        }
        public async Task<User> GetByEmail(string email)
        {
            var user = await db.Users.FirstOrDefaultAsync(a => a.Email == email);
            return user;
        }
  

        public async Task Create(User user)
        {
            await db.Users.AddAsync(user);
        }

        public void Update(User user)
        {
            db.Entry(user).State = EntityState.Modified;
        }

        public async Task Delete(int id)
        {
            User? user = await db.Users.FindAsync(id);
            if (user != null)
                db.Users.Remove(user);
        }
    }
}
