using Campaign.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Campaign;

abstract class Program
{
    static void Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IOrderService, OrderService>()
            .AddSingleton<IProductService, ProductService>()
            .AddSingleton<ICampaignService, CampaignService>()
            .AddSingleton<ICalculateService, CalculateService>()
            .AddSingleton<IExecuteCommandService, ExecuteCommandService>()
            .BuildServiceProvider();
        
        var _executeCommandService = serviceProvider.GetService<IExecuteCommandService>();
        
        Console.WriteLine("For exit: 'exit'");
        string command;
        do
        {
            command = Console.ReadLine();
            _executeCommandService.ExecuteCommand(command);
        } while (command != "exit");
    }
}