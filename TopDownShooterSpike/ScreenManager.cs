using System.Dynamic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TopDownShooterSpike.Screens;

namespace TopDownShooterSpike
{
    public class ScreenManager
    {
        private static ScreenManager _instance;
        public Vector2 Dimensions { get; private set; }
        public ContentManager Content { get; private set; }
        public GameScreen CurrentScreen;
        public XmlManager<GameScreen> XmlGameScreenManager; 

        public static ScreenManager Instance
        {
            get { return _instance ?? (_instance = Initialize()); }
        }

        private static ScreenManager Initialize()
        {
            var s = new ScreenManager()
            {
                Dimensions = new Vector2(640, 480),
                XmlGameScreenManager = new XmlManager<GameScreen>()
            };
            s.XmlGameScreenManager.Type = typeof(SplashScreen);
            s.CurrentScreen = s.XmlGameScreenManager.Load("Content/screens/SplashScreen.xml");

            return s;
        }

        public void LoadContent(ContentManager content)
        {
            Content = new ContentManager(content.ServiceProvider, "Content");
            CurrentScreen.LoadContent();
        }

        public void UnloadContent()
        {
            CurrentScreen.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            CurrentScreen.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            CurrentScreen.Draw(spriteBatch);
        }
    }
}
