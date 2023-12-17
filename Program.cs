using Campaign.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Campaign;

abstract class Program
{
    private static IOrderService _orderService;
    private static IProductService _productService;
    private static ICampaignService _campaignService;
    private static ICalculateService _calculateService;

    static void Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IOrderService, OrderService>()
            .AddSingleton<IProductService, ProductService>()
            .AddSingleton<ICampaignService, CampaignService>()
            .AddSingleton<ICalculateService, CalculateService>()
            .BuildServiceProvider();
        
        _productService = serviceProvider.GetService<IProductService>();
        _campaignService = serviceProvider.GetService<ICampaignService>();
        _orderService = serviceProvider.GetService<IOrderService>();
        _calculateService = serviceProvider.GetService<ICalculateService>();
        
        Console.WriteLine("For exit: 'exit'");
        string command;
        do
        {
            command = Console.ReadLine();
            ExecuteCommand(command);
        } while (command != "exit");
    }

    static void ExecuteCommand(string command)
    {
        var parts = command.Split(' ');
        var action = parts[0];

        switch (action)
        {
            case "create_product":
                _productService.CreateProduct(parts[1], decimal.Parse(parts[2]), int.Parse(parts[3]));
                break;
            case "get_product_info":
                _productService.GetProductInfo(parts[1]);
                break;
            case "create_order":
                _orderService.CreateOrder(parts[1], int.Parse(parts[2]));
                break;
            case "create_campaign":
                CreateCampaign(parts[1], parts[2], int.Parse(parts[3]), decimal.Parse(parts[4]), int.Parse(parts[5]));
                break;
            case "get_campaign_info":
                _campaignService.GetCampaignInfo(parts[1]);
                break;
            case "increase_time":
                IncreaseTime(int.Parse(parts[1]));
                break;
            case "exit":
                break;
            default:
                Console.WriteLine("Invalid command");
                break;
        }
    }

    static void IncreaseTime(int hours)
    {
        TimeService.IncreaseTime(hours);
        _calculateService.CalculateCampaigns();
    }

    static void CreateCampaign(string name, string productCode, int duration, decimal priceManipulationLimit, int targetSalesCount)
    {
        var checkCampaign = _campaignService.CheckCampaign(name);
        if (checkCampaign)
        {
            Console.WriteLine($"There is a campaign named {name}");
            return;
        }

        var product = _productService.GetProduct(productCode);
        if (product != null)
        {
            _campaignService.Add(name, productCode, duration, priceManipulationLimit, targetSalesCount);
            Console.WriteLine($"Campaign created; name {name}, product {productCode}, duration {duration}, limit {priceManipulationLimit}, target sales count {targetSalesCount}");
        }
        else
            Console.WriteLine($"Product {productCode} not found");
    }
}