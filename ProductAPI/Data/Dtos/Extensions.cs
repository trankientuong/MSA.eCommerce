using ProductAPI.Data.Dtos;
using ProductAPI.Data.Entities;

public static class Extensions
{
    public static ProductDto AsDto(this Product product)
    {
        return new ProductDto(
            product.Id,
            product.Name,
            product.Price,
            product.Image
        );
    }
}