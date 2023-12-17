namespace Campaign.Services;

public interface IExecuteCommandService
{
    string IncreaseTime(int hours);
    string ExecuteCommand(string command);
}

public class ExecuteCommandService : IExecuteCommandService
{
    private readonly IOrderService _orderService;
    private readonly IProductService _productService;
    private readonly ICampaignService _campaignService;
    private readonly ICalculateService _calculateService;

    public ExecuteCommandService(IOrderService orderService, IProductService productService, 
        ICampaignService campaignService, ICalculateService calculateService)
    {
        _orderService = orderService;
        _productService = productService;
        _campaignService = campaignService;
        _calculateService = calculateService;
    }

    public string ExecuteCommand(string command)
    {
        var parts = command.Split(' ');
        var action = parts[0];

        switch (action)
        {
            case "create_product":
                return _productService.CreateProduct(parts[1], decimal.Parse(parts[2]), int.Parse(parts[3]));
            case "get_product_info":
                return _productService.GetProductInfo(parts[1]);
            case "create_order":
                return _orderService.CreateOrder(parts[1], int.Parse(parts[2]));
            case "create_campaign":
                return _campaignService.CreateCampaign(parts[1], parts[2], int.Parse(parts[3]), decimal.Parse(parts[4]), int.Parse(parts[5]));
            case "get_campaign_info":
                return _campaignService.GetCampaignInfo(parts[1]);
            case "increase_time":
                return IncreaseTime(int.Parse(parts[1]));
            case "exit":
                break;
            default:
                var msg = "Invalid command";
                Console.WriteLine(msg);
                return msg;
        }

        return "Invalid command";
    }
    
    public string IncreaseTime(int hours)
    {
        var msg = TimeService.IncreaseTime(hours);
        _calculateService.CalculateCampaigns();
        return msg;
    }
}