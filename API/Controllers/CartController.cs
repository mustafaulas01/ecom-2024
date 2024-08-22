using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService=cartService;
        }

        [HttpGet]
        public async Task<ActionResult<ShoppingCart>> GetCartById(string id)
        {
            var cart=await _cartService.GetCartAsync(id);

            return Ok(cart?? new ShoppingCart {Id=id});
        }

         [HttpPost]
         public async Task<ActionResult<ShoppingCart>> UpdateCart(ShoppingCart cart)
        {
            var updatedCart= await _cartService.SetCartAsync(cart);
            if(updatedCart==null)return BadRequest("Problem with cart");

            return updatedCart;
        }

        [HttpDelete]
         public async Task<ActionResult<ShoppingCart>> DeleteCart(string id)
        {
          var result= await _cartService.DeleteCartAsync(id);
          if(!result) return BadRequest("Problem deleting cart");
                 
           return Ok();      
        }
    }
}


