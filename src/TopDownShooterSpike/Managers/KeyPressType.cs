using Microsoft.Xna.Framework.Input;

namespace TopDownShooterSpike.Managers
{
    public class KeyPressType
    {
        public Keys Key;
        public KeyState State;

        public static KeyPressType Down(Keys key)
        {
            return new KeyPressType
            {
                Key = key,
                State = KeyState.Down
            };
        }

        public static KeyPressType Pressed(Keys key)
        {
            return new KeyPressType
            {
                Key = key,
                State = KeyState.Debounced
            };
        }

        public static KeyPressType Up(Keys key)
        {
            return new KeyPressType
            {
                Key = key,
                State = KeyState.Up
            };
        }

        public enum KeyState
        {
            Up = 0,
            Down = 1,
            Debounced = 2,
        }
    }
}