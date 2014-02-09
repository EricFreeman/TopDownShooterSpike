using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TopDownShooterSpike
{
    public class InputManager
    {
        private KeyboardState _currentKeyState, _previousKeyState;
        private MouseState _currentMouseState, _previousMouseState;

        private static InputManager _instance;
        public static InputManager Instance
        {
            get { return _instance ?? (_instance = new InputManager()); }
        }

        public void Update()
        {
            _previousKeyState = _currentKeyState;
            _previousMouseState = _currentMouseState;

            if (!ScreenManager.Instance.IsTransitioning)
            {
                _currentKeyState = Keyboard.GetState();
                _currentMouseState = Mouse.GetState();
            }
        }

        #region Keyboard

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

        #endregion

        #region Mouse

        public Vector2 GetMousePostion()
        {
            return new Vector2(_currentMouseState.X, _currentMouseState.Y);
        }

        public bool LeftMousePressed()
        {
            return _currentMouseState.LeftButton == ButtonState.Pressed &&
                   _previousMouseState.LeftButton == ButtonState.Released;
        }

        public bool LeftMouseHeld()
        {
            return _currentMouseState.LeftButton == ButtonState.Pressed;
        }

        public bool LeftMouseReleased()
        {
            return _currentMouseState.LeftButton == ButtonState.Released &&
                   _previousMouseState.LeftButton == ButtonState.Pressed;
        }

        public bool RightMousePressed()
        {
            return _currentMouseState.RightButton == ButtonState.Pressed &&
                   _previousMouseState.RightButton == ButtonState.Released;
        }

        public bool RightMouseHeld()
        {
            return _currentMouseState.RightButton == ButtonState.Pressed;
        }

        public bool RightMouseReleased()
        {
            return _currentMouseState.RightButton == ButtonState.Released &&
                   _previousMouseState.RightButton == ButtonState.Pressed;
        }

        #endregion
    }
}
