using Campaign.Models;

namespace Campaign.Services;

public interface IOrderService
{
    List<Order> List();
    string CreateOrder(string productCode, int quantity);
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

    public string CreateOrder(string productCode, int quantity)
    {
        var product = _productService.GetProduct(productCode);
        string msg;
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
                msg = $"Order created; product {productCode}, quantity {quantity}";
            }
            else
                msg = $"Insufficient stock for product {productCode}";
        }
        else
            msg = $"Product {productCode} not found";
        
        Console.WriteLine(msg);
        return msg;
    }
}