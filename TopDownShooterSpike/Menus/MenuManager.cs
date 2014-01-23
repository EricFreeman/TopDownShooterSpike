using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TopDownShooterSpike.Menus
{
    public class MenuManager
    {
        private Menu _menu;

        public MenuManager()
        {
            _menu = new Menu();
            _menu.OnMenuChange += menu_OnMenuChange;
        }

        private void menu_OnMenuChange(object sender, EventArgs e)
        {
            var xmlManager = new XmlManager<Menu>();
            _menu.UnloadContent();
            _menu = xmlManager.Load(_menu.Id);
            _menu.LoadContent();
        }

        public void LoadContent(string menuPath)
        {
            if (menuPath != string.Empty)
                _menu.Id = menuPath;
        }

        public void UnloadContent()
        {
            _menu.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            _menu.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _menu.Draw(spriteBatch);
        }
    }
}
