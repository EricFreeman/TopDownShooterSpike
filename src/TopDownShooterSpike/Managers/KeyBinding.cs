using System;

namespace TopDownShooterSpike.Managers
{
    public sealed class KeyBinding
    {
        private readonly KeyPressType[] _keyPresses;
        private readonly Action _callback;

        public KeyBinding(KeyPressType[] keyPresses, Action callbackAction)
        {
            _callback = callbackAction;
            _keyPresses = keyPresses;
        }

        public KeyPressType[] KeyPresses
        {
            get { return _keyPresses; }
        }

        public Action Callback
        {
            get { return _callback; }
        }
    }
}