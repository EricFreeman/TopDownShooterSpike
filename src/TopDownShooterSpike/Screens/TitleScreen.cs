using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TopDownShooterSpike.Managers;

namespace TopDownShooterSpike.Screens
{
    public class TitleScreen : GameScreen
    {
        private MenuManager _menuManager;

        public TitleScreen(Game game) : base(game)
        {
            _menuManager = new MenuManager(Game);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            _menuManager.LoadContent("Content/Menus/TitleMenu.xml");
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            _menuManager.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _menuManager.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            _menuManager.Draw(spriteBatch);
        }
    }
}
