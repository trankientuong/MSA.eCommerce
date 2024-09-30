using Business;
using ProductAPI.Data;
using ProductAPI.Data.Entities;

namespace ProductAPI.Repositories;

public class ProductRepository : GenericRepository<ProductDbContext, Product> 
{
    public ProductRepository(ProductDbContext context) : base(context)
    {        
    }
    
    // Specific method for Product
}