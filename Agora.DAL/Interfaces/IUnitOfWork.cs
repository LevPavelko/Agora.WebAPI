using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.DAL.Entities;

namespace Agora.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Address> Addresses { get; }
        IRepository<Admin> Admins { get; }
        IRepository<BankCard> BankCards { get; }
        IRepository<Brand> Brands { get; }
        IRepository<Cashback> Cashbacks { get; }
        IRepository<Category> Categories { get; }
        IRepository<Country> Countries { get; }
        IRepository<Customer> Customers { get; }
        IRepository<DeliveryOptions> DeliveryOptions { get; }
        IRepository<Discount> Discounts { get; }
        IRepository<FAQ> FAQs { get; }
        IRepository<FAQCategory> FAQCategories { get; }
        IRepository<GiftCard> GiftCards { get; }
        IRepository<Order> Orders { get; }
        //IRepository<OrderItem> OrderItems { get; }
        IRepository<Payment> Payments { get; }
        IRepository<PaymentMethod> PaymentMethods { get; }
        IRepository<Product> Products { get; }
        IRepository<ProductReview> ProductReviews { get; }
        IRepository<Return> Returns { get; }
        //IRepository<ReturnItem> ReturnItems { get; }
        IRepository<Seller> Sellers { get; }
        IRepository<SellerReview> SellerReviews { get; }
        IRepository<Shipping> Shippings { get; }
        IRepository<Store> Stores { get; }
        IRepository<Subcategory> Subcategories { get; }
        IRepository<Support> Supports { get; }
        IRepository<User> Users { get; }
        IRepository<Wishlist> Wishlists { get; }
        Task Save();
    }
}

