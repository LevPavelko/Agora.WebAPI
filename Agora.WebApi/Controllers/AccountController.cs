using System.Threading.Tasks;
using Agora.BLL.DTO;
using Agora.BLL.Interfaces;
using Agora.Models;
using Microsoft.AspNetCore.Mvc;

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

        public AccountController(IUserService userService, ISellerService sellerService, IStoreService storeService, IAddressService addressService, 
            ICountryService countryService, IJWTService JWTService)

        {
            _userService = userService;
            _sellerService = sellerService;
            _storeService = storeService;
            _addressService = addressService;
            _countryService = countryService;
            _JWTService = JWTService;
        }

       
    }
}
