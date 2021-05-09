using System;
using SharpBrick.PoweredUp;
using SharpBrick.PoweredUp.Functions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using SharpDX.XInput;

namespace XboxLegoController
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceCollection = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole())
                .AddPoweredUp()
                .AddWinRTBluetooth(); // using WinRT Bluetooth on Windows (separate NuGet SharpBrick.PoweredUp.WinRT; others are available)
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var host = serviceProvider.GetService<PoweredUpHost>();
            var proxyController = new MainController();
            await proxyController.InitializeAsync(host);

            Console.ReadLine();
        }
    }
}
