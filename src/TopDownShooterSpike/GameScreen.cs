using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TopDownShooterSpike.Managers;

namespace TopDownShooterSpike
{
    public class GameScreen
    {
        protected ContentManager Content;
        public string XmlPath;
        public string Music;

        protected Game Game;

        public GameScreen(Game game)
        {
            XmlPath = "Content/Screens/" + GetType().Name + ".xml";
            Game = game;
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
