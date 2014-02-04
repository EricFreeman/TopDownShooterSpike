using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TopDownShooterSpike
{
    public class GameScreen
    {
        protected ContentManager Content;
        public string XmlPath;
        public string Music;

        public GameScreen()
        {
            XmlPath = "Content/Screens/" + GetType().Name + ".xml";
        }

        public virtual void LoadContent()
        {
            Content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");

            if(Music != string.Empty)
                AudioManager.Instance.FadeInBackgroundMusic(Music);
            else
                AudioManager.Instance.FadeOutBackgroundMusic();
        }

        public virtual void UnloadContent()
        {
            Content.Unload();
        }

        public virtual void Update(GameTime gameTime)
        {
            InputManager.Instance.Update();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            
        }
    }
}
