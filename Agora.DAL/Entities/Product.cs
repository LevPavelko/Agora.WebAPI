﻿
namespace Agora.DAL.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public decimal Rating { get; set; }

        public virtual Subcategory? Subcategory { get; set; }
        public virtual Category? Category { get; set; }
        public virtual Brand? Brand { get; set; }
        public virtual Store? Store { get; set; }
        public virtual Order? Order { get; set; }
        public virtual Discount? Discount { get; set; }
        public virtual ICollection<Wishlist>? Wishlists { get; set; }
        public virtual ICollection<ProductReview>? ProductReviews { get; set; }
    }
}
