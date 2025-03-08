using Agora.Enums;

namespace Agora.DAL.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public OrderStatus Status { get; set; } 
        public DateOnly PaymentDeadline { get; set; }

        public int? ProductId { get; set; }
        public virtual Product? Product { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual Payment? Payment { get; set; }
        public virtual Shipping? Shipping { get; set; }  
        public virtual Return? Return { get; set; }
    }
}
