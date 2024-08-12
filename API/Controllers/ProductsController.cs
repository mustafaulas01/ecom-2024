using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {


        private readonly StoreContext _context;

        public ProductsController(StoreContext context)
        {
           _context=context; 
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetPrdoucts()
        {
         return await _context.Products.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>>GetProduct(int id)
        {
            var product=await _context.Products.FindAsync(id);

           if(product==null) return NotFound();

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>>CreateProduct(Product model)
        {
          _context.Products.Add(model);
          await _context.SaveChangesAsync();


          return Ok(model);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id,Product model)
        {
            if(model.Id!=id ||!ProductExists(id)) return  BadRequest("Can not update this product");
 
           _context.Entry(model).State=EntityState.Modified;
           await _context.SaveChangesAsync();

           return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(a=>a.Id==id);
        }

        [HttpDelete("{id:int}")]

        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product=await _context.Products.FindAsync(id);
            if(product==null) return NotFound();

            _context.Products.Remove(product);

            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}
