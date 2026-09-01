using System;
using System.Runtime.InteropServices;

namespace RoboTwin.UI.Core.Input
{
    /// <summary>
    /// SDL2-based Industrial Gamepad & RC Transmitter Manager.
    /// Bypasses standard WPF/Avalonia input to support custom 6-axis space mice,
    /// FrSky/FlySky RC controllers, and standard industrial joysticks.
    /// </summary>
    public class GamepadManager
    {
        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        private static extern int SDL_Init(uint flags);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        private static extern int SDL_NumJoysticks();

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        private static extern nint SDL_JoystickOpen(int device_index);

        [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
        private static extern short SDL_JoystickGetAxis(nint joystick, int axis);

        private const uint SDL_INIT_JOYSTICK = 0x00000200;
        private nint _activeJoystick = nint.Zero;

        public GamepadManager()
        {
            if (SDL_Init(SDL_INIT_JOYSTICK) < 0)
            {
                throw new Exception("Failed to initialize SDL2 Hardware Input Subsystem.");
            }
        }

        public bool ConnectPrimary()
        {
            if (SDL_NumJoysticks() > 0)
            {
                _activeJoystick = SDL_JoystickOpen(0);
                return _activeJoystick != nint.Zero;
            }
            return false;
        }

        /// <summary>
        /// Reads physical axis values (e.g., Twist commands for ROS2)
        /// Applies deadzones to prevent drift.
        /// </summary>
        public (double LinearX, double AngularZ) ReadTwistCommand(short deadzone = 4000)
        {
            if (_activeJoystick == nint.Zero) return (0, 0);

            short axisY = SDL_JoystickGetAxis(_activeJoystick, 1); // Up/Down
            short axisX = SDL_JoystickGetAxis(_activeJoystick, 2); // Left/Right Twist

            double linearX = Math.Abs(axisY) > deadzone ? -axisY / 32767.0 : 0.0;
            double angularZ = Math.Abs(axisX) > deadzone ? axisX / 32767.0 : 0.0;

            return (linearX, angularZ);
        }
    }
}
