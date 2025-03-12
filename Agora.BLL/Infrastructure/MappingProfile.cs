using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.BLL.DTO;
using Agora.DAL.Entities;
using AutoMapper;

namespace Agora.BLL.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<Wishlist, WishlistDTO>();
            CreateMap<Support, SupportDTO>();
            CreateMap<Subcategory, SubcategoryDTO>();
            CreateMap<Store, StoreDTO>();
            CreateMap<Shipping, ShippingDTO>();
            CreateMap<Seller, SellerDTO>();
            CreateMap<SellerReview, SellerReviewDTO>();
            CreateMap<Return, ReturnDTO>();
            CreateMap<Product, ProductDTO>();
            CreateMap<Payment, PaymentDTO>();
            CreateMap<PaymentMethod, PaymentMethodDTO>();
            CreateMap<Order, OrderDTO>();
            CreateMap<OrderItem, OrderItemDTO>();
        }
    }
}
