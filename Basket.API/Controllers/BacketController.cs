using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BacketController : Controller
    {
        private readonly IBasketRepository _repository;
        private readonly DiscountGrpcService _discountServiceGrpc;


        public BacketController(IBasketRepository repository, DiscountGrpcService discountServiceGrpc)
        {
            _repository = repository ?? 
                throw new ArgumentNullException(nameof(repository));

            _discountServiceGrpc = discountServiceGrpc ?? 
                throw new ArgumentNullException(nameof(discountServiceGrpc));
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
                var coupon = await _discountServiceGrpc.GetDiscount(item.ProductName);
                if (coupon == null) continue;
                item.Price -= coupon.Amount;
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
