using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.Enums;

namespace Agora.BLL.DTO
{
    public class SippingDTO //????
    {
        public int Id { get; set; }
        public ShippingStatus Status { get; set; }
        public string? TrackingNumber { get; set; }
        //public AddressDTO Address { get; set; }
        public int? OrderId { get; set; }
        public int? DeliveryOptionsId { get; set; }
    }
}
