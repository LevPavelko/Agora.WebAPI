using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.Enums;

namespace Agora.BLL.DTO
{
    public class OrderDTO //??
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public OrderStatus Status { get; set; }
        public DateOnly PaymentDeadline { get; set; }
        public int? ProductId { get; set; }
        //public ProductDTO Product { get; set; }
        //public CustomerDTO Customer { get; set; }
        //public PaymentDTO Payment { get; set; }
        //public ShippingDTO Shipping { get; set; }
        //public ReturnDTO Return { get; set; }

    }
}
