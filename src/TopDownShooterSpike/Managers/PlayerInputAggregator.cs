using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TopDownShooterSpike.Managers
{
    public class PlayerInputAggregator : GameComponent, IPlayerInputAggregator
    {
        private readonly IList<KeyBinding> _bindings;

        private KeyboardState _oldKeyState, _newKeyState;

        public PlayerInputAggregator(Game game) : base(game)
        {
            _bindings = new List<KeyBinding>();
            _oldKeyState = _newKeyState = Keyboard.GetState();

            UpdateOrder = int.MinValue;
            game.Components.Add(this);
        }

        private void Insert(Action callback, params KeyPressType[] type)
        {
            _bindings.Add(new KeyBinding(type, callback));
        }

        public void Up(Keys key, Action callback)
        {
            Insert(callback, KeyPressType.Up(key));
        }

        public void Pressed(Keys key, Action callback)
        {
            
            Insert(callback, KeyPressType.Pressed(key));
        }

        public void Down(Keys key, Action callback)
        {
            Insert(callback, KeyPressType.Down(key));
        }

        public void OnCombination(Action callback, params KeyPressType[] keyPressTypes)
        {
            Insert(callback, keyPressTypes);
        }

        public bool Active(KeyPressType kpt)
        {
            var key = kpt.Key;

            // compose a wrapper function that is deferred..
            var callback = new Func<Func<Keys, bool>, Func<bool>>(function => (() => function(key)));
            Func<bool> composed = null;

            switch (kpt.State)
            {
                case KeyPressType.KeyState.Up: composed = callback(Up);
                    break;
                case KeyPressType.KeyState.Down: composed = callback(Down);
                    break;
                case KeyPressType.KeyState.Debounced: composed = callback(Pressed);
                    break;
            }

            // until we want the return value
            return composed.Execute();
        }

        private bool Pressed(Keys key)
        {
            return _oldKeyState.IsKeyUp(key) && _newKeyState.IsKeyDown(key);
        }

        private bool Down(Keys key)
        {
            return _newKeyState.IsKeyDown(key);
        }

        private bool Up(Keys key)
        {
            return _newKeyState.IsKeyUp(key);
        }

        public override void Update(GameTime gameTime)
        {
            _oldKeyState = _newKeyState;
            _newKeyState = Keyboard.GetState();

            foreach (var keyBinding in _bindings)
            {
                var presses = keyBinding.KeyPresses;

								if(presses.Any(Active))
									keyBinding.Callback.Execute();
            }
        }
    }
}