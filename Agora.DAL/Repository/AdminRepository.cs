using Agora.DAL.EF;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Agora.DAL.Repository
{
    public class AdminRepository : IRepository<Admin>
    {
        private AgoraContext db;
        public AdminRepository(AgoraContext context)
        {
            this.db = context;
        }

        public async Task<IQueryable<Admin>> GetAll()
        {
            return db.Admins;
        }

        public async Task<Admin> Get(int id)
        {
            return await db.Admins.FindAsync(id);
        }

        public async Task Create(Admin admin)
        {
            await db.Admins.AddAsync(admin);
        }

        public void Update(Admin admin)
        {
            db.Entry(admin).State = EntityState.Modified;
        }

        public async Task Delete(int id)
        {
            Admin? admin = await db.Admins.FindAsync(id);
            if (admin != null)
                db.Admins.Remove(admin);
        }
    }
}