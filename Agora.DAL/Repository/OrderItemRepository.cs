using Agora.DAL.EF;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Agora.DAL.Repository
{
    public class OrderItemRepository: IRepository<OrderItem>
    {
        private AgoraContext db;
        public OrderItemRepository(AgoraContext context)
        {
            this.db = context;
        }

        public async Task<IQueryable<OrderItem>> GetAll()
        {
            return db.OrderItems;
        }

        public async Task<OrderItem> Get(int id)
        {
            return await db.OrderItems.FindAsync(id);
        }

        public async Task Create(OrderItem orderItem)
        {
            await db.OrderItems.AddAsync(orderItem);
        }

        public void Update(OrderItem orderItem)
        {
            db.Entry(orderItem).State = EntityState.Modified;
        }

        public async Task Delete(int id)
        {
            OrderItem? orderItem = await db.OrderItems.FindAsync(id);
            if (orderItem != null)
                db.OrderItems.Remove(orderItem);
        }
    }
}
