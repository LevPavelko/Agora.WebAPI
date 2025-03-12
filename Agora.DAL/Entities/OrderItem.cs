
namespace Agora.DAL.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }
        public decimal PriceAtMoment { get; set; }
        public int Quantity { get; set; }

        public virtual Shipping? Shipping { get; set; }
        public virtual Order? Order { get; set; }
        public virtual Product? Product { get; set; }
    }
}
