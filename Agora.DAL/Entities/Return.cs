using Agora.Enums;

namespace Agora.DAL.Entities
{
    public class Return
    {
        public int Id { get; set; }
        public DateOnly ReturnDate { get; set; }
        public ReturnStatus Status{ get; set; } 
        public decimal RefundAmount { get; set; }
        public int Quantity { get; set; }
        public string? Reason { get; set; }

        public virtual Order? Order { get; set; }       
        public virtual Customer? Customer { get; set; }
    }
}
