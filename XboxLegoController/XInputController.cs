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
        private byte _prevLeftTrigger;
        private byte _prevRightTrigger;
        public event ControllerEventHandler OnLeftThumbChanged;
        public event ControllerEventHandler OnRightTriggerChanged;
        private TopGearRallyCar _car;
        private int _prevSpeed = 0;
        private int _prevAngle = 0;

        public XInputController(TopGearRallyCar car)
        {
            _car = car;

            // Initialize XInput
            Controllers = new[] { new Controller(UserIndex.One), new Controller(UserIndex.Two), new Controller(UserIndex.Three), new Controller(UserIndex.Four) };
        }

        private async Task ButtonChangeHandler()
        {
            if (State.Gamepad.Buttons.HasFlag(GamepadButtonFlags.A))
                await _car.SetDriveSpeed(100);
            else if (State.Gamepad.Buttons.HasFlag(GamepadButtonFlags.B))
                await _car.SetDriveSpeed(-100);
            else
                await _car.SetDriveSpeed(0);
        }

        private async Task TriggerChangeHandler()
        {
            int newSpeed = 0;
            if (State.Gamepad.LeftTrigger == 0 || State.Gamepad.RightTrigger == 0)
            {
                if (State.Gamepad.LeftTrigger > 0)
                {
                    newSpeed = - CalcSpeed(State.Gamepad.LeftTrigger);
                }
                else
                {
                    newSpeed = CalcSpeed(State.Gamepad.RightTrigger);
                }
            }
            if (newSpeed != _prevSpeed)
            {
                await _car.SetDriveSpeed(newSpeed);
                _prevSpeed = newSpeed;
            }
        }

        private async Task ThumbChangeHandler()
        {
            var newAngle = CalcAngle(State.Gamepad.LeftThumbX);
            if (newAngle != _prevAngle)
            {
                await _car.SetSteeringDegrees(newAngle);
                _prevAngle = newAngle;
            }

        }

        private int CalcSpeed(int trigger)
        {
            return trigger * 100 / 255;
        }

        private int CalcAngle(int thumbstick)
        {
            return thumbstick * 45 / 32768;
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
                    _prevLeftTrigger = previousState.Gamepad.LeftTrigger;
                    _prevRightTrigger = previousState.Gamepad.RightTrigger;
                    while (controller.IsConnected)
                    {
                        State = controller.GetState();
                        if (previousState.PacketNumber != State.PacketNumber)
                        {
                            if (_prevButtons != State.Gamepad.Buttons)
                            {
                                _prevButtons = State.Gamepad.Buttons;
                                await ButtonChangeHandler();
                            }
                            if (_prevLeftThumbX != State.Gamepad.LeftThumbX)
                            {
                                _prevLeftThumbX = State.Gamepad.LeftThumbX;
                                await ThumbChangeHandler();
                            }
                            if (_prevLeftTrigger != State.Gamepad.LeftTrigger || _prevRightTrigger != State.Gamepad.RightTrigger)
                            {
                                _prevRightTrigger = State.Gamepad.RightTrigger;
                                _prevLeftTrigger = State.Gamepad.LeftTrigger;
                                await TriggerChangeHandler();
                            }
                        }
                        await Task.Delay(50);
                        previousState = State;
                    }
                }
            }
        } 
    }
}