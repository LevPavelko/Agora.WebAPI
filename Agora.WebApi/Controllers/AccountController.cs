using Agora.BLL.DTO;
using Agora.BLL.Interfaces;
using Agora.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Konscious.Security.Cryptography;

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

        public AccountController(IUserService userService, ISellerService sellerService, IStoreService storeService, IAddressService addressService, ICountryService countryService)
        {
            _userService = userService;
            _sellerService = sellerService;
            _storeService = storeService;
            _addressService = addressService;
            _countryService = countryService;
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

                return CreatedAtAction(nameof(RegisterSeller), new { id = seller.Id }, seller);
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
    }
}
