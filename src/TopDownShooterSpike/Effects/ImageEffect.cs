using Microsoft.Xna.Framework;

namespace TopDownShooterSpike.Effects
{
    public class ImageEffect
    {
        #region Properties

        protected Image _image;
        public bool IsActive;

        #endregion

        #region Constructor

        public ImageEffect()
        {
            IsActive = false;
        }

        #endregion

        #region Hooks

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

        #endregion
    }
}
