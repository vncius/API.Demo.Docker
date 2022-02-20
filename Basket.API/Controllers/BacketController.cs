using Basket.API.DTOs;
using Basket.API.Entities;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BacketController : Controller
    {
        private readonly string _baseAddress;
        private readonly IBasketRepository _repository;
        private readonly HttpClient _client;

        public BacketController(IBasketRepository repository, IConfiguration configuration)
        {
            _repository = repository ?? 
                throw new ArgumentNullException(nameof(repository));

            _client = new HttpClient();

            _baseAddress = configuration.GetValue<string>("DiscountSettings:DiscountUrl");
        }

        [HttpGet("{userName}", Name = "GetBasket")]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        { 
            var basket = await _repository.GetBasket(userName);

            return Ok(basket ?? new ShoppingCart(userName));
        }

        [HttpPost]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
        {
            foreach (var item in basket.Items)
            {
                var url = $"{_baseAddress}/Discount/{item.ProductName}";
                HttpResponseMessage response = await _client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var coupon = await response.Content.ReadFromJsonAsync<Coupon>();
                    if (coupon == null) continue;
                    item.Price -= coupon.Amount;
                }
            }

            return Ok(await _repository.UpdateBasket(basket));
        }

        [HttpDelete("{userName}", Name = "DeleteBasket")]
        public async Task<ActionResult<ShoppingCart>> DeleteBasket(string userName)
        {
            await _repository.DeleteBasket(userName);
            return StatusCode(202);
        }
    }
}
