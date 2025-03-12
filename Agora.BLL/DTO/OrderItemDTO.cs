using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agora.BLL.DTO
{
    public class OrderItemDTO
    {
        public int Id { get; set; }
        public decimal PriceAtMoment { get; set; }
        public int Quantity { get; set; }
    }
}
