namespace Contracts.Events.Cart;

public record CartCreated
{
    public string UserId { get; set; } = string.Empty;
    public IEnumerable<CartDetails> cartDetails { get; set; }
};  

public record CartDetails
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}