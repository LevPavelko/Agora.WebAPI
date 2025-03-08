using Agora.Enums;

namespace Agora.DAL.Entities
{
    public class Shipping
    {
        public int Id { get; set; }
        public ShippingStatus Status { get; set; }                                            
        public string? TrackingNumber { get; set; }
        
        public virtual Address? Address { get; set; }

        public int? OrderId { get; set; }
        public virtual Order? Order { get; set; }

        public int? DeliveryOptionsId { get; set; }
        public virtual DeliveryOptions? DeliveryOptions { get; set; }
    }
}
