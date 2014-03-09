using System;
using Microsoft.Xna.Framework.Input;

namespace TopDownShooterSpike.Managers
{
    public class InputManager : IDeviceInputService
    {
        public bool KeyDown(Keys key)
        {
            throw new NotImplementedException();
        }

        public bool KeyUp(Keys key)
        {
            throw new NotImplementedException();
        }

        public bool KeyPress(Keys key)
        {
            throw new NotImplementedException();
        }
    }

    public interface IDeviceInputService
    {
        bool KeyDown(Keys key);
        bool KeyUp(Keys key);
        bool KeyPress(Keys key);
    }
}
