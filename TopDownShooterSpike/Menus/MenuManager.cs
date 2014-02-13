using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TopDownShooterSpike.Managers;

namespace TopDownShooterSpike.Menus
{
    public class MenuManager
    {
        #region Properties

        private Menu _menu;
        private bool _isTransitioning;

        #endregion

        #region Constructor

        public MenuManager()
        {
            _menu = new Menu();
            _menu.OnMenuChange += menu_OnMenuChange;
        }

        #endregion

        #region Events

        private void menu_OnMenuChange(object sender, EventArgs e)
        {
            var xmlManager = new XmlManager<Menu>();
            _menu.UnloadContent();
            _menu = xmlManager.Load(_menu.Id);
            _menu.LoadContent();
            _menu.OnMenuChange += menu_OnMenuChange;
            _menu.Transition(0f);

            foreach (var item in _menu.Items)
            {
                item.Image.StoreEffects();
                item.Image.ActivateEffect("FadeEffect");
            }
        }

        #endregion

        #region Transition

        private void Transition(GameTime gameTime)
        {
            if (!_isTransitioning) return;
            foreach (MenuItem i in _menu.Items)
            {
                i.Image.Update(gameTime);
                float first = _menu.Items[0].Image.Alpha;
                float last = _menu.Items[_menu.Items.Count - 1].Image.Alpha;
                if (first == 0f && last == 0f)
                    _menu.Id = _menu.Items[_menu.ItemNumber].LinkId;
                else if (first == 1f && last == 1f)
                {
                    _isTransitioning = false;
                    foreach (var item in _menu.Items)
                        item.Image.RestoreEffects();
                }
            }
        }

        #endregion

        #region Hooks

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
            if(!_isTransitioning)
                _menu.Update(gameTime);

            if (InputManager.Instance.KeyPressed(Keys.Enter) && !_isTransitioning)
            {
                if (_menu.Items[_menu.ItemNumber].LinkType == "Screen")
                    ScreenManager.Instance.ChangeScreens(_menu.Items[_menu.ItemNumber].LinkId);
                else
                {
                    _isTransitioning = true;
                    _menu.Transition(1f);
                    foreach (var item in _menu.Items)
                    {
                        item.Image.StoreEffects();
                        item.Image.ActivateEffect("FadeEffect");
                    }
                }
            }
            Transition(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _menu.Draw(spriteBatch);
        }

        #endregion
    }
}
