using Campaign.Models;

namespace Campaign.Services;

public interface ICampaignService
{
    List<ProductCampaign> List();
    void GetCampaignInfo(string name);
    ProductCampaign? GetCampaign(string name);
    bool CheckCampaign(string campaignName);
    ProductCampaign? GetActiveCampaign(string productCode);
    void Add(string name, string productCode, int duration, decimal priceManipulationLimit, int targetSalesCount);
}

public class CampaignService : ICampaignService
{
    private static readonly List<ProductCampaign> Campaigns = new();

    public void Add(string name, string productCode, int duration, decimal priceManipulationLimit, int targetSalesCount)
    {
        var currentTime = TimeService.GetCurrentTime();
        
        Campaigns.Add(new ProductCampaign
        {
            Name = name,
            Turnover = 0,
            TotalSales = 0,
            Duration = duration,
            ProductCode = productCode,
            TargetSalesCount = targetSalesCount,
            StartTime = currentTime,
            EndTime = currentTime.AddHours(duration),
            PriceManipulationLimit = priceManipulationLimit,
        });
    }

    public bool CheckCampaign(string campaignName)
    {
        var checkCampaign = Campaigns.Any(a => a.Name == campaignName);
        return checkCampaign;
    }

    public ProductCampaign? GetCampaign(string name)
    {
        var campaign = Campaigns.FirstOrDefault(c => c.Name == name);
        return campaign;
    }
    
    public ProductCampaign? GetActiveCampaign(string productCode)
    {
        var currentTime = TimeService.GetCurrentTime();
        
        return Campaigns.FirstOrDefault(campaign =>
            campaign.ProductCode == productCode && 
            currentTime >= campaign.StartTime &&
            currentTime <= campaign.EndTime);
    }
    
    public void GetCampaignInfo(string name)
    {
        var campaign = GetCampaign(name);
        if (campaign != null)
        {
            var currentTime = TimeService.GetCurrentTime();
            var status = campaign.EndTime > currentTime ? "Active" : "Ended";
            var averageItemPrice = campaign.TotalSales > 0 ? campaign.Turnover / campaign.TotalSales : 0;
            Console.WriteLine($"Campaign {name} info; Status {status}, Target Sales {campaign.TargetSalesCount}, Total Sales {campaign.TotalSales}, Turnover {campaign.Turnover}, Average Item Price {averageItemPrice}");
        }
        else
            Console.WriteLine($"Campaign {name} not found");
    }

    public List<ProductCampaign> List()
    {
        return Campaigns;
    }
}