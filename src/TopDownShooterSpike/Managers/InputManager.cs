using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TopDownShooterSpike.Managers
{
    public class InputManager : GameComponent, IDeviceInputService
    {
        private KeyboardState _newKeyboardState, _oldKeyboardState;
        private GamePadState _newGamePadState, _oldGamePadState;

        public InputManager(Game game) : base(game)
        {
            game.Components.Add(this);
        }

        public override void Update(GameTime gameTime)
        {
            _oldKeyboardState = _newKeyboardState;
            _newKeyboardState = Keyboard.GetState();

            _oldGamePadState = _newGamePadState;
            _newGamePadState = GamePad.GetState(PlayerIndex.One, GamePadDeadZone.Circular);
        }

        public bool KeyDown(Keys key)
        {
            return _newKeyboardState.IsKeyDown(key);
        }

        public bool KeyUp(Keys key)
        {
            return _newKeyboardState.IsKeyUp(key);
        }

        public bool KeyPress(Keys key)
        {
            return _newKeyboardState.IsKeyUp(key) && _oldKeyboardState.IsKeyDown(key);
        }
    }
}
