using Microsoft.EntityFrameworkCore;
using Agora.BLL.Infrastructure;
using Agora.BLL.Interfaces;
using Agora.BLL.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Scalar.AspNetCore;
using StackExchange.Redis;


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
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAddressService, AddressService>();
            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped<IBankCardService, BankCardService>();
            builder.Services.AddScoped<IBrandService, BrandService>();
            builder.Services.AddScoped<ICashbackService, CashbackService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ICountryService, CountryService>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<IDeliveryOptionsService, DeliveryOptionsService>();
            builder.Services.AddScoped<IDiscountService, DiscountService>();
            builder.Services.AddScoped<IFAQCategoryService, FAQCategoryService>();
            builder.Services.AddScoped<IFAQService, FAQService>();
            builder.Services.AddScoped<IGiftCardService, GiftCardService>();
            builder.Services.AddScoped<IOrderItemService, OrderItemService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IPaymentMethodService, PaymentMethodService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IReturnItemService, ReturnItemService>();
            builder.Services.AddScoped<IReturnService, ReturnService>();
            builder.Services.AddScoped<ISellerReviewService, SellerReviewService>();
            builder.Services.AddScoped<ISellerService, SellerService>();
            builder.Services.AddScoped<IShippingService, ShippingService>();
            builder.Services.AddScoped<IStoreService, StoreService>();
            builder.Services.AddScoped<ISubcategoryService, SubcategoryService>();
            builder.Services.AddScoped<ISupportService, SupportService>();
            builder.Services.AddScoped<IWishlistService, WishlistService>();
            builder.Services.AddScoped<ISecureService, SecureService>();
            builder.Services.AddScoped<IStatisticsService, StatisticsService>();
            builder.Services.AddHostedService<StatisticsCacheService>();

            // for Redis caching:
            try
            {
                var redis = ConnectionMultiplexer.Connect("localhost:6379");
                builder.Services.AddSingleton<IConnectionMultiplexer>(redis);
            }            
            catch
            {
                Console.WriteLine("Redis not connected..");
                builder.Services.AddSingleton<IConnectionMultiplexer>(sp => null);
            }            

            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            //JWT
            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", policy =>
                {
                    policy.WithOrigins("http://localhost:3000", "http://localhost:5193")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });
            


            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();  // Scalar UI will be available at: http://localhost:5193/scalar/v1
            }

            app.UseCors("AllowSpecificOrigin");
            app.UseAuthentication();  
            app.UseAuthorization(); 
            //app.UseMiddleware<JwtValidationMiddleware>();
            //app.UseHttpsRedirection();
            app.UseRouting();
            app.UseSession();
    
            app.UseStaticFiles();
            //app.UseCors(builder => builder.WithOrigins("http://localhost:3000", "http://localhost:5193")// for React и Scalar
            //                           .AllowAnyHeader()
            //                           .AllowAnyMethod()
            //                            .AllowCredentials());

            app.MapControllers();

            
            app.Run();
            



        }
    }
}
