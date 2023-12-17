using Campaign.Services;

namespace Campaign.Test;

public class CampaignTest
{
    private OrderService _orderService;
    private ProductService _productService;
    private CampaignService _campaignService;
    private CalculateService _calculateService;
    private ExecuteCommandService _executeCommandService;
    
    public CampaignTest()
    {
        _productService = new ProductService();
        _campaignService = new CampaignService(_productService);
        _orderService = new OrderService(_productService, _campaignService);
        _calculateService = new CalculateService(_orderService, _productService, _campaignService);
        _executeCommandService =
            new ExecuteCommandService(_orderService, _productService, _campaignService, _calculateService);
    }
    
    [Fact]
    public async Task CampaignFlow()
    {
        var commandList = new List<string>
        {
            "create_product",
            "create_campaign",
            "increase_time",
            "create_order",
            "increase_time",
            "create_order",
            "increase_time",
            "increase_time",
            "increase_time",
            "get_campaign_info",
            "get_product_info",
        };

        foreach (var command in commandList)
        {
            switch (command)
            {
                case "create_product":
                    CreateProduct("create_product P1 100 55");
                    await Task.Delay(5000);
                    break;
                case "create_campaign":
                    CreateCampaign("create_campaign C1 P1 5 20 100");
                    await Task.Delay(5000);
                    break;
                case "create_order":
                    CreateOrder("create_order P1 2");
                    await Task.Delay(5000);
                    break;
                case "increase_time":
                    IncreaseTime("increase_time 1");
                    await Task.Delay(5000);
                    break;
                case "get_campaign_info":
                    CampaignInfo("get_campaign_info C1");
                    await Task.Delay(5000);
                    break;
                case "get_product_info":
                    ProductInfo("get_product_info P1");
                    await Task.Delay(5000);
                    break;
            }
        }
    }

    [Theory, InlineData("create_product P1 100 55")]
    public void CreateProduct(string command)
    {
        const string expected = "Product created; code P1, price 100, stock 55";
        var result = _executeCommandService.ExecuteCommand(command);
        Assert.Equal(expected, result);
    }

    // [Theory, InlineData("create_campaign C1 P1 5 20 100")]
    public void CreateCampaign(string command)
    {
        const string expected = "Campaign created; name C1, product P1, duration 5, limit 20, target sales count 100";
        var result = _executeCommandService.ExecuteCommand(command);
        Assert.Equal(expected, result);
    }

    // [Theory, InlineData("create_order P1 2")]
    public void CreateOrder(string command)
    {
        const string expected = "Order created; product P1, quantity 2";
        var result = _executeCommandService.ExecuteCommand(command);
        Assert.Equal(expected, result);
    }

    // [Theory, InlineData("increase_time 1")]
    public void IncreaseTime(string command)
    {
        const string expected = "Time is";
        var result = _executeCommandService.ExecuteCommand(command);
        Assert.StartsWith(expected, result);
    }

    // [Theory, InlineData("get_campaign_info C1")]
    public void CampaignInfo(string command)
    {
        command = "create_campaign C1 P1 5 20 100";
        _executeCommandService.ExecuteCommand(command);
        
        const string expected = "Campaign C1 info; Status";
        command = "get_campaign_info C1";
        var result = _executeCommandService.ExecuteCommand(command);
        Assert.StartsWith(expected, result);
    }

    // [Theory, InlineData("get_product_info P1")]
    public void ProductInfo(string command)
    {
        const string expected = "Product P1 info; price";
        var result = _executeCommandService.ExecuteCommand(command);
        Assert.StartsWith(expected, result);
    }
}