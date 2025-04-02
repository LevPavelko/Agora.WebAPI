using System.Threading.Tasks;
using Agora.BLL.Infrastructure;
using Agora.BLL.DTO;
using Agora.BLL.Interfaces;
using Agora.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Konscious.Security.Cryptography;
using System.ComponentModel.DataAnnotations;
using Agora.DAL.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Agora.Controllers
{
    [ApiController]
    [Route("api/account")]
    
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ISellerService _sellerService;
        private readonly IStoreService _storeService;
        private readonly IAddressService _addressService;
        private readonly ICountryService _countryService;
        private readonly IJWTService _JWTService;
        private readonly ICustomerService _customerService;
        private readonly IConfiguration _config;

        public AccountController(IUserService userService, ISellerService sellerService, IStoreService storeService, IAddressService addressService, 
            ICountryService countryService, IJWTService JWTService, ICustomerService customerService, IConfiguration config)

        {
            _userService = userService;
            _sellerService = sellerService;
            _storeService = storeService;
            _addressService = addressService;
            _countryService = countryService;
            _JWTService = JWTService;
            _customerService = customerService;
            _config = config;
        }

        [HttpPost("register-seller")]
        public async Task<IActionResult> RegisterSeller(RegSellerViewModel regSeller)
        {             
            try
            {                
                string hashedPassword = HashPassword(regSeller.Password);

                UserDTO userDTO = new UserDTO();
                userDTO.Name = regSeller.Name;
                userDTO.Surname = regSeller.Surname;
                userDTO.PhoneNumber = regSeller.PhoneNumber;
                userDTO.Password = hashedPassword;
                userDTO.Email = regSeller.Email;

                var userId = await _userService.CreateReturnId(userDTO);

                SellerDTO sellerDTO = new SellerDTO();
                sellerDTO.UserId = userId; 

                int sellerId = await _sellerService.Create(sellerDTO);
                
                StoreDTO storeDTO = new StoreDTO();
                storeDTO.Name = regSeller.StoreName;
                storeDTO.CreatedAt = DateOnly.FromDateTime(DateTime.Now);
                storeDTO.SellerId = sellerId;
                
                await _storeService.Create(storeDTO);
                
                var country = await _countryService.Get(regSeller.CountryId);

                if (country == null) return BadRequest("Invalid country ID.");

                AddressDTO addressDTO = new AddressDTO();
                
                addressDTO.Appartement = regSeller.Appartement;
                addressDTO.Building = regSeller.Building;
                addressDTO.Street = regSeller.Street;
                addressDTO.City = regSeller.City;
                addressDTO.PostalCode = regSeller.PostalCode;
                addressDTO.CountryId = country.Id;

                await _addressService.Create(addressDTO);

                var seller = await _sellerService.Get(sellerId);
                var role = await _userService.GetRoleByUserId(userDTO.Id);
                string jwtToken = _JWTService.GenerateJwtToken(userDTO, role);
                Response.Cookies.Append("jwt", jwtToken, new CookieOptions //добавление HTTP Only куки
                {
                    HttpOnly = true,
                    Secure = true, //  Если HTTPS то true
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddMinutes(30)
                });


                return CreatedAtAction(nameof(RegisterSeller), new { id = seller.Id }, seller);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("register-user")]
        public async Task<IActionResult> RegisterUser(RegUserViewModel regUser)
        {
            try
            {
                string hashedPassword = HashPassword(regUser.Password);

                UserDTO userDTO = new UserDTO
                {
                    Name = regUser.Name,
                    Surname = regUser.Surname,
                    PhoneNumber = regUser.PhoneNumber,
                    Password = hashedPassword,
                    Email = regUser.Email
                };

                var userId = await _userService.CreateReturnId(userDTO);

                var user = await _userService.Get(userId);

                var role = await _userService.GetRoleByUserId(user.Id);
                string jwtToken = _JWTService.GenerateJwtToken(user, role);
                Response.Cookies.Append("jwt", jwtToken, new CookieOptions //добавление HTTP Only куки
                {
                    HttpOnly = true,
                    Secure = true, //  Если HTTPS то true
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddMinutes(30)
                });


                bool customerCreated = await _customerService.CreateForUser(userId);
                if (!customerCreated)
                {
                    return StatusCode(500, "Error occurred while creating customer record.");
                }

                return CreatedAtAction(nameof(RegisterUser), new { id = user.Id }, new { user, jwtToken });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        private string HashPassword(string password)
        {
            using var hasher = new Argon2id(Encoding.UTF8.GetBytes(password));
            hasher.DegreeOfParallelism = 8;
            hasher.MemorySize = 65536;
            hasher.Iterations = 4;
            return Convert.ToBase64String(hasher.GetBytes(32));
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginModel model)
        {
            UserDTO user = null;

            try
            {
                user = await _userService.GetByEmail(model.Email);
            }
            catch (ValidationExceptionFromService ex)
            {
                if (ex.Message == "There is no user with this email")
                {
                    user = null;
                }
                else
                {
                    return StatusCode(500, new { message = "Internal Server Error" });
                }
            }

            if (user != null)
            {
                if (user.GoogleId == "")
                {
                    return new JsonResult(new { message = "You already have an account, but it's not linked to Google." }) { StatusCode = 400 };
                }

                if (user.Email.Equals(model.Email) && user.GoogleId == model.GoogleId)
                {
                    var role = await _userService.GetRoleByUserId(user.Id);
                    string jwtToken = _JWTService.GenerateJwtToken(user, role);
                    Response.Cookies.Append("jwt", jwtToken, new CookieOptions //добавление HTTP Only куки
                    {
                        HttpOnly = true,
                        Secure = true, //  Если HTTPS то true
                        SameSite = SameSiteMode.None,
                        Expires = DateTime.UtcNow.AddMinutes(30)
                    });

                    return Ok(new { jwtToken });
                }
                else
                {
                    return new JsonResult(new { message = "Google ID does not match." }) { StatusCode = 400 };
                }
            }
            else
            {
                var newUser = new UserDTO
                {
                    Name = model.Name,
                    Email = model.Email,
                    GoogleId = model.GoogleId
                };

                bool result = await _userService.CreateGoogle(newUser);
                if (result)
                {
                    var role = await _userService.GetRoleByUserId(user.Id);
                    string jwtToken = _JWTService.GenerateJwtToken(user, role);
                    Response.Cookies.Append("jwt", jwtToken, new CookieOptions //добавление HTTP Only куки
                    {
                        HttpOnly = true,
                        Secure = true, //  Если HTTPS то true
                        SameSite = SameSiteMode.None,
                        Expires = DateTime.UtcNow.AddMinutes(30)
                    });

                    return Ok(new { jwtToken });
                }
                else
                {
                    return new JsonResult(new { message = "Error occurred during registration." }) { StatusCode = 500 };
                }
            }
           
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                UserDTO user = await _userService.GetByEmail(model.Email);
                if (user == null)
                    return new JsonResult(new { message = "You don't have account! Sing up!" }) { StatusCode = 401 };
                string hashedPass = HashPassword(model.Password);
                if (user.Email.Equals(model.Email) && user.Password.Equals(hashedPass))
                {
                    var role = await _userService.GetRoleByUserId(user.Id);
                    string jwtToken = _JWTService.GenerateJwtToken(user, role);
                    Response.Cookies.Append("jwt", jwtToken, new CookieOptions //добавление HTTP Only куки
                    {
                        HttpOnly = true,
                        Secure = true, //  Если HTTPS то true
                        SameSite = SameSiteMode.None,
                        Expires = DateTime.UtcNow.AddMinutes(30)
                    });

                    return Ok(new { message = "Authenticated" });
                }
                else
                {
                    return new JsonResult(new { message = "Wrong password or email!" }) { StatusCode = 403 };
                }

            }
            catch (ValidationExceptionFromService ex) 
            {
                return new JsonResult(new { message = "You have to sing up first!" }) { StatusCode = 401 };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new JsonResult(new { message = "Server error!" }) { StatusCode = 500 };
            }


        }

        [HttpGet("get-user-role")]  
        public IActionResult GetUserRole()
        {
            var authHeader = HttpContext.Request.Headers["JWT"].FirstOrDefault();
            string token = null;

            if (!string.IsNullOrEmpty(authHeader))
            {
                token = authHeader;
            }

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("No token provided.");
            }


            try
            {
                var roleDTO = _JWTService.DecryptJwtToken(token);
                return Ok(new { Role = roleDTO.Role });
            }
            catch (SecurityTokenException)
            {
                return Unauthorized("Invalid token.");
            }
        }
    }
}
