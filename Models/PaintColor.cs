namespace CarBuilder.Models;

public class Paint
{
    public int Id { get; set; }
    public decimal Price { get; set; }
    public string Color { get; set; }
    public List<Order> Orders { get; set; }
}