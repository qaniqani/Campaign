using Campaign.Models;

namespace Campaign.Services;

public interface ICalculateService
{
    void CalculateCampaigns();
}

public class CalculateService : ICalculateService
{
    private readonly IOrderService _orderService;
    private readonly IProductService _productService;
    private readonly ICampaignService _campaignService;

    public CalculateService(IOrderService orderService, IProductService productService, ICampaignService campaignService)
    {
        _orderService = orderService;
        _productService = productService;
        _campaignService = campaignService;
    }
    
    public void CalculateCampaigns()
    {
        var orders = _orderService.List();
        var products = _productService.List();
        var campaigns = _campaignService.List();
        foreach (var campaign in campaigns)
        {
            campaign.TotalSales = 0;
            campaign.Turnover = 0;
            foreach (var product in products)
            {
                var newPrice = CalculateNewPrice(product, campaign);
                // Console.WriteLine($"New product price: {newPrice}");
                
                var orderGrp = orders
                    .Where(a => a.CampaingCode == campaign.Name && a.ProductCode == product.Code)
                    .GroupBy(a => new
                    {
                        a.CampaingCode,
                        a.ProductCode,
                        a.OrderPrice
                    })
                    .Select(a => new
                    {
                        a.Key.OrderPrice,
                        a.Key.ProductCode,
                        a.Key.CampaingCode,
                        TotalSaleCount = a.Sum(s => s.Quantity),
                    }).ToList();
                
                foreach (var order in orderGrp)
                {
                    var salesCount = order.TotalSaleCount;
                    campaign.TotalSales += salesCount;
                    campaign.Turnover += salesCount * order.OrderPrice;
                    product.Price = newPrice;
                    product.Stock -= salesCount;
                }
            }
        }
    }
    
    private decimal CalculateNewPrice(Product product, ProductCampaign campaign)
    {
        var minPrice = product.MainPrice * (1 - campaign.PriceManipulationLimit / 100);
        var maxPrice = product.MainPrice * (1 + campaign.PriceManipulationLimit / 100);

        var newPrice = CalculatePrice(maxPrice, minPrice);

        return newPrice;
    }

    private decimal CalculatePrice(decimal maxPrice, decimal minPrice)
    {
        var random = new Random();
        return (decimal)(random.NextDouble() * (double)(maxPrice - minPrice)) + minPrice;
    }
}