using System;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ProductRepository : IProductRepository
{

    private readonly StoreContext _context;

    public ProductRepository(StoreContext context)
    {
        _context=context;
    }
    public void AddProduct(Product product)
    {
       _context.Products.Add(product);
    }

    public void DeleteProduct(Product product)
    {
       _context.Products.Remove(product);
    }

    public async Task<IReadOnlyList<string>> GetBrandAsync()
    {
        return await _context.Products.Select(a=>a.Brand).Distinct().ToListAsync();
    }

    public async  Task<Product?> GetProductByIdAsync(int id)
    {
       return await _context.Products.FindAsync(id);
    }

    public  async Task<IReadOnlyList<Product>> GetProductsAsync(string? brand,string? type,string? sort)
    {
        var query=_context.Products.AsQueryable();

        if(!string.IsNullOrWhiteSpace(brand))
        query=query.Where(a=>a.Brand==brand);

        if(!string.IsNullOrWhiteSpace(type))
        query=query.Where(a=>a.Type==type);

       
         //sort
            query=sort switch
            {
                "priceAsc" => query.OrderBy(x =>x.Price),
                "priceDesc"=>query.OrderByDescending(x=>x.Price),
                _ => query.OrderBy(x=>x.Name)
            };
      

        return await query.ToListAsync();
    }

    public async  Task<IReadOnlyList<string>> GetTypesAsync()
    {
      return await _context.Products.Select(a=>a.Type).Distinct().ToListAsync();
    }

    public bool ProductExists(int id)
    {
       return _context.Products.Any(a=>a.Id==id);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await  _context.SaveChangesAsync()>0;
    }

    public void UpdateProduct(Product product)
    {
        _context.Entry(product).State=EntityState.Modified;
    }
}
