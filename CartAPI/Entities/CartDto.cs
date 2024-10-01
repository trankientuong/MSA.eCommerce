namespace CartAPI.Entities;

public class CartDto
{
    public string UserId { get; set; } = string.Empty;
    public IEnumerable<CartDetailsDto> CartDetailsDtos { get; set; }
}

public class CartDetailsDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string Image { get; set; } = string.Empty;
}