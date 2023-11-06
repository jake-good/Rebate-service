using System;
using Microsoft.Extensions.DependencyInjection;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Runner;

class Program
{
    static void Main(string[] args)
    {
        IServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<IRebateDataStore, RebateDataStore>();
        serviceCollection.AddSingleton<IProductDataStore, ProductDataStore>();
        serviceCollection.AddSingleton<ICalculatorFactory, CalculatorFactory>();
        serviceCollection.AddSingleton<IRebateService, RebateService>();
        IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

        var rebateService = serviceProvider.GetService<IRebateService>();

        Console.Write("Enter Rebate Identifier: ");
        string rebateIdentifier = Console.ReadLine();

        Console.Write("Enter Product Identifier: ");
        string productIdentifier = Console.ReadLine();

        Console.Write("Enter Volume: ");

        if (!decimal.TryParse(Console.ReadLine(), out decimal volume))
        {
            Console.WriteLine("Invalid volume");
            return;
        };

        var request = new CalculateRebateRequest
        {
            RebateIdentifier = rebateIdentifier,
            ProductIdentifier = productIdentifier,
            Volume = volume,
        };

        var result = rebateService.Calculate(request);
        if (result.Success)
        {
            Console.WriteLine($"Rebate calculation successful. Rebate amount: ${result.Amount}");
        }
        else
        {
            Console.WriteLine($"Rebate calculation failed. Reason: {result.Reason}");
        }
    }
}
