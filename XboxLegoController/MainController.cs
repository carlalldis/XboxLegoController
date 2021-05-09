using SharpBrick.PoweredUp;
using SharpDX.XInput;
using System;
using System.Threading.Tasks;

namespace XboxLegoController
{
    class MainController
    {
        private PoweredUpHost _host;
        private TechnicMediumHub _hub;
        private XInputController _controller;
        private TopGearRallyCar _car;

        public MainController()
        {
        }

        public async Task InitializeAsync(PoweredUpHost host)
        {
            _host = host;

            Console.WriteLine("Discovering Technic Hub");
            _hub = await _host.DiscoverAsync<TechnicMediumHub>();
            await _hub.ConnectAsync();

            Console.WriteLine("Initializing Car");
            _car = new TopGearRallyCar(_hub);
            await _car.InitializeAsync();

            Console.WriteLine("Initializing Controller");
            _controller = new XInputController(_car);
            await _controller.InitializeAsync();
        }
    }
}