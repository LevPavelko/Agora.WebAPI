
namespace Agora.DAL.Entities
{
    public class PaymentMethod
    {
        public int Id { get; set; }
        
        public virtual Payment? Payment { get; set; }         
        public virtual BankCard? BankCard { get; set; }       
        public virtual GiftCard? GiftCard { get; set; }        
        public virtual Cashback? Cashback { get; set; }       
    }
}
