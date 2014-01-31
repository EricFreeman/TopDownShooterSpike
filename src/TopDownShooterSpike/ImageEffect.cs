using Microsoft.Xna.Framework;

namespace TopDownShooterSpike
{
    public class ImageEffect
    {
        protected Image _image;
        public bool IsActive;

        public ImageEffect()
        {
            IsActive = false;
        }

        public virtual void LoadContent(ref Image image)
        {
            _image = image;
        }

        public virtual void UnloadContent()
        {
            
        }

        public virtual void Update(GameTime gameTime)
        {
            
        }
    }
}
