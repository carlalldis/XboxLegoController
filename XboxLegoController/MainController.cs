using SharpBrick.PoweredUp;
using SharpDX.XInput;
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

            _hub = await _host.DiscoverAsync<TechnicMediumHub>();
            await _hub.ConnectAsync();

            _car = new TopGearRallyCar(_hub);
            await _car.InitializeAsync();

            _controller = new XInputController(_car);
            await _controller.InitializeAsync();
        }
    }
}