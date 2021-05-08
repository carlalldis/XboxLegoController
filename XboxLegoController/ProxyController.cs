using SharpBrick.PoweredUp;
using SharpDX.XInput;
using System.Threading.Tasks;

namespace XboxLegoController
{
    class ProxyController
    {
        private PoweredUpHost _host;
        private TechnicMediumHub _hub;
        private XInputController _controller;
        private TopGearRallyCar _car;

        public ProxyController()
        {
        }

        public async Task InitializeAsync(PoweredUpHost host)
        {
            _host = host;

            _controller = new XInputController();
            _ = _controller.InitializeAsync();

            _hub = await _host.DiscoverAsync<TechnicMediumHub>();
            await _hub.ConnectAsync();

            _car = new TopGearRallyCar(_hub);
            await _car.InitializeAsync();

            _controller.OnButtonChanged += ButtonChangeHandler;
        }

        private async void ButtonChangeHandler()
        {
            if (_controller.State.Gamepad.Buttons.HasFlag(GamepadButtonFlags.A))
                await _car.SetDriveSpeed(100);
            else if (_controller.State.Gamepad.Buttons.HasFlag(GamepadButtonFlags.B))
                await _car.SetDriveSpeed(-100);
            else
                await _car.SetDriveSpeed(0);
        }
    }
}