using Microsoft.EntityFrameworkCore;
using Agora.BLL.Infrastructure;
using Agora.BLL.Interfaces;
using Agora.BLL.Services;
using Scalar.AspNetCore;

namespace Agora
{
  public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();
            builder.Services.AddAgoraContext(connection);

            builder.Services.AddUnitOfWorkService();
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<IAddressService, AddressService>();
            builder.Services.AddTransient<IAdminService, AdminService>();
            builder.Services.AddTransient<IBankCardService, BankCardService>();
            builder.Services.AddTransient<IBrandService, BrandService>();
            builder.Services.AddTransient<ICashbackService, CashbackService>();
            builder.Services.AddTransient<ICategoryService, CategoryService>();
            builder.Services.AddTransient<ICountryService, CountryService>();
            builder.Services.AddTransient<ICustomerService, CustomerService>();
            builder.Services.AddTransient<IDeliveryOptionsService, DeliveryOptionsService>();
            builder.Services.AddTransient<IDiscountService, DiscountService>();
            builder.Services.AddTransient<IFAQCategoryService, FAQCategoryService>();
            builder.Services.AddTransient<IFAQService, FAQService>();
            builder.Services.AddTransient<IGiftCardService, GiftCardService>();
            builder.Services.AddTransient<IOrderItemService, OrderItemService>();
            builder.Services.AddTransient<IOrderService, OrderService>();
            builder.Services.AddTransient<IPaymentMethodService, PaymentMethodService>();
            builder.Services.AddTransient<IPaymentService, PaymentService>();
            builder.Services.AddTransient<IProductService, ProductService>();
            builder.Services.AddTransient<IReturnItemService, ReturnItemService>();
            builder.Services.AddTransient<IReturnService, ReturnService>();
            builder.Services.AddTransient<ISellerReviewService, SellerReviewService>();
            builder.Services.AddTransient<ISellerService, SellerService>();
            builder.Services.AddTransient<IShippingService, ShippingService>();
            builder.Services.AddTransient<IStoreService, StoreService>();
            builder.Services.AddTransient<ISubcategoryService, SubcategoryService>();
            builder.Services.AddTransient<ISupportService, SupportService>();
            builder.Services.AddTransient<IWishlistService, WishlistService>();

            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }
            app.UseAuthorization();
            //app.UseHttpsRedirection();
            app.UseRouting();
            app.UseSession();
          
            app.UseStaticFiles();
            app.UseCors(builder => builder.WithOrigins("http://localhost:3000", "http://localhost:5193")// for React и Scalar
                                       .AllowAnyHeader()
                                       .AllowAnyMethod()
                                        .AllowCredentials());

            app.MapControllers();

            app.Run();

        }
    }
}
