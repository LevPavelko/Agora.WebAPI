using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.Enums;

namespace Agora.BLL.DTO
{
    public class ReturnDTO
    {
        public int Id { get; set; }
        public DateOnly ReturnDate { get; set; }
        public ReturnStatus Status { get; set; }
        public decimal RefundAmount { get; set; }
        public int Quantity { get; set; }
        public string? Reason { get; set; }

        public int? OrderId { get; set; }
        //public OrderDTO Order { get; set; }
        //public CustomerDTO Customer { get; set; }
    }
}
