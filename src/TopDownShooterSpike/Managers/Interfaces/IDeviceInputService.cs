using Microsoft.Xna.Framework.Input;

namespace TopDownShooterSpike.Managers
{
    public interface IDeviceInputService
    {
        bool KeyDown(Keys key);
        bool KeyUp(Keys key);
        bool KeyPress(Keys key);
    }
}