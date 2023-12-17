namespace Campaign.Models;

public class ProductCampaign
{
    public string Name { get; set; }
    public string ProductCode { get; set; }
    public int Duration { get; set; }
    public decimal PriceManipulationLimit { get; set; }
    public int TargetSalesCount { get; set; }

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public int TotalSales { get; set; }
    public decimal Turnover { get; set; }
}