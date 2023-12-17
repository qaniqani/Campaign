namespace Campaign.Models;

public class Order
{
    public int Quantity { get; set; }
    public decimal OrderPrice { get; set; }
    public string ProductCode { get; set; }
    public string CampaingCode { get; set; }
}