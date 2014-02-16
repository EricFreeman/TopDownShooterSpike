using Microsoft.Xna.Framework;

namespace TopDownShooterSpike.Effects
{
    public class FadeEffect : ImageEffect
    {
        public float FadeSpeed;
        public bool Increase;

        public FadeEffect()
        {
            FadeSpeed = 1;
            Increase = false;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (_image.IsActive)
            {
                if (!Increase)
                    _image.Alpha -= FadeSpeed*(float) gameTime.ElapsedGameTime.TotalSeconds;
                else
                    _image.Alpha += FadeSpeed*(float) gameTime.ElapsedGameTime.TotalSeconds;

                if (_image.Alpha < 0f)
                {
                    Increase = true;
                    _image.Alpha = 0f;
                }
                else if(_image.Alpha > 1f)
                {
                    Increase = false;
                    _image.Alpha = 1f;
                }
            }
            else
            {
                _image.Alpha = 1f;
            }
        }
    }
}
