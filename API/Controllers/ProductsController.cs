using Core.Entities;
using Core.Interfaces;
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


        private readonly IProductRepository _productRepo;

        public ProductsController(IProductRepository repository)
        {
           _productRepo=repository;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand,string? type,string? sort)
        {
         return Ok(await _productRepo.GetProductsAsync(brand,type,sort));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>>GetProduct(int id)
        {
            var product=await _productRepo.GetProductByIdAsync(id);

           if(product==null) return NotFound();

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>>CreateProduct(Product model)
        {
           _productRepo.AddProduct(model);
          
        if( await _productRepo.SaveChangesAsync())
        {
            return CreatedAtAction("GetProduct",new {id=model.Id},model );
        }


          return BadRequest("Problem creating product"); 
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id,Product model)
        {
            if(model.Id!=id ||!_productRepo.ProductExists(id)) return  BadRequest("Can not update this product");
 
            _productRepo.UpdateProduct(model);

          if(  await _productRepo.SaveChangesAsync())
          {
            return NoContent();
          }
          

           return  BadRequest("Promlem updating the product");
        }


        [HttpDelete("{id:int}")]

        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product=await _productRepo.GetProductByIdAsync(id);
            if(product==null) return NotFound();

           _productRepo.DeleteProduct(product);

                if(  await _productRepo.SaveChangesAsync())
          {
            return NoContent();
          }
         return  BadRequest("Promlem deleting the product");
        }

       [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>>GetBrands()
        {

          return Ok( await _productRepo.GetBrandAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>>GetTypes()
        {

          return Ok( await _productRepo.GetTypesAsync());
        }

    }
}
