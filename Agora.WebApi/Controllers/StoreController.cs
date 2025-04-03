using Agora.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Agora.Controllers
{
    [Route("api/stores")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IStoreService _storeService;

        public StoreController(IStoreService storeService)
        {
            _storeService = storeService;
        }

        [HttpGet("{sellerId}/stores")]
        public async Task<IActionResult> GetSellerStores(int sellerId)
        {
            var stores = await _storeService.GetAll();
            var sellerStores = stores.Where(r => r.SellerId == sellerId).ToList();

            return Ok(sellerStores);
        }
    }
}
