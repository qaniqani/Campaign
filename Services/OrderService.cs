using Campaign.Models;

namespace Campaign.Services;

public interface IOrderService
{
    List<Order> List();
    void CreateOrder(string productCode, int quantity);
}

public class OrderService : IOrderService
{
    private readonly IProductService _productService;
    private readonly ICampaignService _campaignService;
    
    private static readonly List<Order> Orders = new();

    public OrderService(IProductService productService, ICampaignService campaignService)
    {
        _productService = productService;
        _campaignService = campaignService;
    }

    public List<Order> List()
    {
        return Orders;
    }

    public void CreateOrder(string productCode, int quantity)
    {
        var product = _productService.GetProduct(productCode);
        if (product != null)
        {
            if (product.Stock >= quantity)
            {
                var activeCampaign = _campaignService.GetActiveCampaign(product.Code);
                var price = activeCampaign != null ? product.Price : product.MainPrice;
                Orders.Add(new Order
                {
                    OrderPrice = price,
                    Quantity = quantity,
                    ProductCode = productCode,
                    CampaingCode = activeCampaign?.Name ?? "-",
                });
                product.Stock -= quantity;
                Console.WriteLine($"Order created; product {productCode}, quantity {quantity}");
            }
            else
                Console.WriteLine($"Insufficient stock for product {productCode}");
        }
        else
            Console.WriteLine($"Product {productCode} not found");
    }
}