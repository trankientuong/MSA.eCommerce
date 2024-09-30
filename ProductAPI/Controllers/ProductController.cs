using Business.IServices;
using Contracts.Domain;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Data.Dtos;
using ProductAPI.Data.Entities;

namespace ProductAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase 
{
    private readonly IRepository<Product> _repository;
    private readonly IFileService _fileService;

    public ProductController(IRepository<Product> repository, IFileService fileService)
    {
        _repository = repository;
        _fileService = fileService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAsync() 
    {
        var products = await _repository.GetAllAsync();
        return Ok(products);
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync([FromForm] CreateProductDto productDto)
    {
        var product = new Product
        {
            Name = productDto.Name,
            Price = productDto.Price
        };

        if (productDto.Image != null) 
        {
            product.Image = await _fileService.SaveFile(productDto.Image, "images/products");
        }

        await _repository.AddAsync(product);

        return CreatedAtAction(nameof(PostAsync), product.AsDto());
    }
}