
namespace Agora.DAL.Entities
{
    public class Seller
    {
        public int Id { get; set; }
        public float Rating { get; set; }

        public virtual User? User { get; set; }
        public virtual ICollection<Store>? Stores { get; set; }
        public virtual ICollection<SellerReview>? SellerReviews { get; set; }
    }
}
