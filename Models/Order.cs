namespace CarBuilder.Models;

public class Order
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public int WheelId { get; set; }
    public int TechnologyId { get; set; }
    public int PaintId { get; set; }
    public int InteriorId { get; set; }
    public Wheels Wheels { get; set; }
    public Interior Interior { get; set; }
    public Paint Paint { get; set; }
    public Technology Technology { get; set; }
    public bool Fulfilled { get; set; }
    public decimal TotalCost 
    {
        get
        {
            decimal totalCost = 0;

            if (Wheels != null)
                totalCost += Wheels.Price;

            if (Technology != null)
                totalCost += Technology.Price;

            if (Paint != null)
                totalCost += Paint.Price;

            if (Interior != null)
                totalCost += Interior.Price;

            return totalCost;
        }
    }
}