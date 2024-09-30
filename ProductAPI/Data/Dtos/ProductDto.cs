using System.ComponentModel.DataAnnotations;

namespace ProductAPI.Data.Dtos;

public record ProductDto
(
    int Id,
    string Name,
    decimal Price,
    string Image
);

public record CreateProductDto
(
    [Required] string Name,
    [Range(0, 1000)] Decimal Price,
    [Required] IFormFile Image
);