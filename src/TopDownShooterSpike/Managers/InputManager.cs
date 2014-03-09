using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace TopDownShooterSpike.Managers
{
    public class InputManager : IDeviceInputService
    {
    }

    public interface IDeviceInputService
    {
        bool KeyDown(Keys key);
        bool KeyUp(Keys key);
        bool KeyPress(Keys key);
    }
}
