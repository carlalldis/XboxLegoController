using SharpDX.XInput;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace XboxLegoController
{
    public delegate void ControllerEventHandler();

    class XInputController
    {
        private Controller[] Controllers;
        public State State;
        private GamepadButtonFlags _prevButtons;
        private short _prevLeftThumbX;
        private byte _prevRightTrigger;
        public event ControllerEventHandler OnButtonChanged;
        public event ControllerEventHandler OnLeftThumbChanged;
        public event ControllerEventHandler OnRightTriggerChanged;

        public XInputController()
        {
            Console.WriteLine("Start XGamepadApp");

            // Initialize XInput
            Controllers = new[] { new Controller(UserIndex.One), new Controller(UserIndex.Two), new Controller(UserIndex.Three), new Controller(UserIndex.Four) };
        }

        public async Task InitializeAsync()
        {
            while (true)
            {
                // Get 1st controller available
                Controller controller = null;
                foreach (var selectController in Controllers)
                {
                    if (selectController.IsConnected)
                    {
                        controller = selectController;
                        break;
                    }
                }

                if (controller == null)
                {
                    Console.WriteLine("No XInput controller found");
                }
                else
                {
                    var test = State.Gamepad.RightTrigger;

                    Console.WriteLine("Found a XInput controller available");
                    Console.WriteLine("Press buttons on the controller to display events or escape key to exit... ");

                    // Poll events from joystick
                    var previousState = controller.GetState();
                    _prevButtons = previousState.Gamepad.Buttons;
                    _prevLeftThumbX = previousState.Gamepad.LeftThumbX;
                    _prevRightTrigger = previousState.Gamepad.RightTrigger;
                    while (controller.IsConnected)
                    {
                        State = controller.GetState();
                        if (previousState.PacketNumber != State.PacketNumber)
                        {
                            if (_prevButtons != State.Gamepad.Buttons)
                            {
                                _prevButtons = State.Gamepad.Buttons;
                                if (OnButtonChanged != null)
                                    OnButtonChanged();
                            }
                            if (_prevLeftThumbX != State.Gamepad.LeftThumbX)
                            {
                                _prevLeftThumbX = State.Gamepad.LeftThumbX;
                                if (OnLeftThumbChanged != null)
                                    OnLeftThumbChanged();
                            }
                            if (_prevRightTrigger != State.Gamepad.RightTrigger)
                            {
                                _prevRightTrigger = State.Gamepad.RightTrigger;
                                if (OnRightTriggerChanged != null)
                                    OnRightTriggerChanged();
                            }
                            Console.WriteLine(State.Gamepad);
                        }
                        await Task.Delay(10);
                        previousState = State;
                    }
                }
            }
        } 
    }
}