using System.Reflection.Metadata.Ecma335;
using API.Models;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {


        private readonly IGenericRepository<Product> _productRepo;
        private readonly IProductRepository _oldProductRepo;

        public ProductsController(IGenericRepository<Product> repository,IProductRepository  productRepository)
        {
           _productRepo=repository;
           _oldProductRepo=productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand,string? type,string? sort,string? search,
         int pageSize=10,int pageIndex=1)
        {
           IQueryable<Product> list = Enumerable.Empty<Product>().AsQueryable();
           list=_productRepo.GetAll();
          
          //Filtering
           if(!string.IsNullOrWhiteSpace(type))
           {
            string[] new_type=type.Split(',');
            if(new_type.Length>1)
            {
                list=list.Where(a=>new_type.Contains(a.Brand));
            }
            else {  list= list.Where(a=>a.Type==type);}
  
           }
                    
           if(!string.IsNullOrWhiteSpace(brand))
           {
            string[]new_brand=brand.Split(',');
            if(new_brand.Length>1)
            {
             list=list.Where(a=>new_brand.Contains(a.Brand));
            }
            else { list= list.Where(a=>a.Brand==brand); }

           }

         //Sorting
         if(!string.IsNullOrWhiteSpace(sort))
         {
             //sort
            list=sort switch
            {
                "priceAsc" => list.OrderBy(x =>x.Price),
                "priceDesc"=>list.OrderByDescending(x=>x.Price),
                _ => list.OrderBy(x=>x.Name)
            };
    
         }
         //search
         if(!string.IsNullOrWhiteSpace(search))
         list=list.Where(a=>a.Name.ToLower().Contains(search.ToLower()));
         //Pagination
           
          int  count=list.Count();  
         var skipResults=(pageIndex-1)*pageSize;

         list=list.Skip(skipResults).Take(pageSize);
         var responseList=   await list.ToListAsync();
        

         var responseData=new ResponseData<Product>(pageIndex,pageSize,count,responseList);

         return Ok(responseData) ;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>>GetProduct(int id)
        {
            var product=await _productRepo.GetByIdAsync(id);

           if(product==null) return NotFound();

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>>CreateProduct(Product model)
        {
           _productRepo.Add(model);
          
        if( await _productRepo.SaveAllAsync())
        {
            return CreatedAtAction("GetProduct",new {id=model.Id},model );
        }

          return BadRequest("Problem creating product"); 
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id,Product model)
        {
            if(model.Id!=id ||!_productRepo.Exists(id)) return  BadRequest("Can not update this product");
 
            _productRepo.Update(model);

          if(  await _productRepo.SaveAllAsync())
          {
            return NoContent();
          }
          

           return  BadRequest("Promlem updating the product");
        }


        [HttpDelete("{id:int}")]

        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product=await _productRepo.GetByIdAsync(id);
            if(product==null) return NotFound();

           _productRepo.Remove(product);

                if(  await _productRepo.SaveAllAsync())
          {
            return NoContent();
          }
         return  BadRequest("Promlem deleting the product");
        }

       [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>>GetBrands()
        {

          return Ok( await _oldProductRepo.GetBrandAsync() );
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>>GetTypes()
        {

          return Ok(await _oldProductRepo.GetTypesAsync());
        }

    }
}
