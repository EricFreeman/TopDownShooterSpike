using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace TopDownShooterSpike
{
    public class InputManager
    {
        private KeyboardState _currentKeyState, _previousKeyState;

        private static InputManager _instance;

        public static InputManager Instance
        {
            get { return _instance ?? (_instance = new InputManager()); }
        }

        public void Update()
        {
            _previousKeyState = _currentKeyState;
            if (!ScreenManager.Instance.IsTransitioning)
                _currentKeyState = Keyboard.GetState();
        }

        public bool KeyPressed(params Keys[] keys)
        {
            return keys.Any(key => _currentKeyState.IsKeyDown(key) && _previousKeyState.IsKeyUp(key));
        }

        public bool KeyReleased(params Keys[] keys)
        {
            return keys.Any(key => _currentKeyState.IsKeyUp(key) && _previousKeyState.IsKeyDown(key));
        }

        public bool KeyDown(params Keys[] keys)
        {
            return keys.Any(key => _currentKeyState.IsKeyDown(key));
        }
    }
}
