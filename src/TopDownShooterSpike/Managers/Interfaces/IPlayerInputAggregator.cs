using System;
using Microsoft.Xna.Framework.Input;

namespace TopDownShooterSpike.Managers
{
    public interface IPlayerInputAggregator
    {
        void Up(Keys key, Action callback);
        void Pressed(Keys key, Action callback);
        void Down(Keys key, Action callback);
        void OnCombination(Action callback, params KeyPressType[] keyPressTypes);
    }
}
