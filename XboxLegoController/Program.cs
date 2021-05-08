using System;
using SharpBrick.PoweredUp;
using SharpBrick.PoweredUp.Functions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

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

            var hub = await host.DiscoverAsync<TechnicMediumHub>();
            await hub.ConnectAsync();

            var Car = new TopGearRallyCar(hub);
            await Car.Initialize();

            Console.WriteLine("Speed 100");
            await Car.SetDriveSpeed(10);
            Console.ReadLine();
            Console.WriteLine("Speed -100");
            await Car.SetDriveSpeed(-10);
            Console.ReadLine();
            Console.WriteLine("Speed 0");
            await Car.SetDriveSpeed(0);
            Console.ReadLine();
            await hub.SwitchOffAsync();
        }

        private static async Task<string> GetInputAsync()
        {
            return await Task.Run(() => Console.ReadLine());
        }
    }
}
