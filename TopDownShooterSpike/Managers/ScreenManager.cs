using System;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TopDownShooterSpike.GameHelpers;
using TopDownShooterSpike.Screens;

namespace TopDownShooterSpike
{
    public class ScreenManager
    {
        private static ScreenManager _instance;
        public Vector2 Dimensions { get; set; }
        [XmlIgnore]
        public ContentManager Content { get; private set; }
        public GameScreen CurrentScreen, NewScreen;
        public XmlManager<GameScreen> XmlGameScreenManager;

        [XmlIgnore]
        public GraphicsDevice GraphicsDevice;
        [XmlIgnore]
        public SpriteBatch SpriteBatch;

        public Image Image;
        [XmlIgnore]
        public bool IsTransitioning { get; private set; }

        [XmlIgnore] 
        public Camera Camera;

        public static ScreenManager Instance
        {
            get { return _instance ?? (_instance = Initialize()); }
        }

        private static ScreenManager Initialize()
        {
            var xml = new XmlManager<ScreenManager>();
            var s = xml.Load("Content/Config/ScreenManager.xml");
            s.XmlGameScreenManager = new XmlManager<GameScreen>();
            s.XmlGameScreenManager.Type = typeof (TempGame);
            s.CurrentScreen = s.XmlGameScreenManager.Load("Content/Screens/TempGame.xml");
            s.Camera = new Camera();
            return s;
        }

        public void ChangeScreens(string screenName)
        {
            var screen = Type.GetType(GetType().Namespace + ".Screens." + screenName);
            NewScreen = (GameScreen) Activator.CreateInstance(screen);
            Image.IsActive = true;
            Image.FadeEffect.Increase = true;
            Image.Alpha = 0f;
            IsTransitioning = true;
        }

        private void Transition(GameTime gameTime)
        {
            if (IsTransitioning)
            {
                Image.Update(gameTime);
                if (Image.Alpha == 1f)
                {
                    CurrentScreen.UnloadContent();
                    CurrentScreen = NewScreen;
                    XmlGameScreenManager.Type = NewScreen.GetType();

                    if(File.Exists(CurrentScreen.XmlPath))
                        CurrentScreen = XmlGameScreenManager.Load(CurrentScreen.XmlPath);

                    CurrentScreen.LoadContent();
                }
                else if(Image.Alpha == 0f)
                {
                    Image.IsActive = false;
                    IsTransitioning = false;
                }
            }
        }

        public void LoadContent(ContentManager content)
        {
            Content = new ContentManager(content.ServiceProvider, "Content");
            CurrentScreen.LoadContent();
            Image.LoadContent();
            Camera.Pos = new Vector2(GraphicsDevice.Viewport.Width/2f, GraphicsDevice.Viewport.Height/2f);
        }

        public void UnloadContent()
        {
            CurrentScreen.UnloadContent();
            if(NewScreen != null)
                NewScreen.UnloadContent();
            Image.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            CurrentScreen.Update(gameTime);
            Transition(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            CurrentScreen.Draw(spriteBatch);
            if(IsTransitioning)
                Image.Draw(spriteBatch);
        }
    }
}
