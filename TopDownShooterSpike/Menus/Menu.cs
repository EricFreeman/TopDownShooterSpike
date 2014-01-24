using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TopDownShooterSpike.Menus
{
    public class Menu
    {
        public event EventHandler OnMenuChange;
        public string Axis;
        public string Effects;

        [XmlElement("Item")] 
        public List<MenuItem> Items;

        private int _itemNumber;
        private string _id;

        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnMenuChange(this, null);
            }
        }

        public int ItemNumber
        {
            get { return _itemNumber; }
        }

        public void Transition(float alpha)
        {
            foreach (var item in Items)
            {
                item.Image.IsActive = true;
                item.Image.Alpha = alpha;
                item.Image.FadeEffect.Increase = alpha == 0f;
            }
        }

        private void AlignMenuItems()
        {
            Vector2 dimensions = Items.Aggregate(Vector2.Zero, (current, item) => current + new Vector2(item.Image.SourceRect.Width, item.Image.SourceRect.Height));

            dimensions = new Vector2((ScreenManager.Instance.Dimensions.X - dimensions.X)/2, (ScreenManager.Instance.Dimensions.Y - dimensions.Y)/2);

            foreach (var item in Items)
            {
                if(Axis == "X")
                    item.Image.Position = new Vector2(dimensions.X, (ScreenManager.Instance.Dimensions.Y - item.Image.SourceRect.Height)/2);
                else if(Axis == "Y")
                    item.Image.Position = new Vector2((ScreenManager.Instance.Dimensions.X - item.Image.SourceRect.Width)/2, dimensions.Y);

                dimensions += new Vector2(item.Image.SourceRect.Width, item.Image.SourceRect.Height);
            }
        }

        public Menu()
        {
            _id = string.Empty;
            _itemNumber = 0;
            Effects = string.Empty;
            Axis = "Y";
            Items = new List<MenuItem>();
        }

        public void LoadContent()
        {
            string[] split = Effects.Split(':');
            foreach (var item in Items)
            {
                item.Image.LoadContent();
                foreach (var s in split)
                {
                    item.Image.ActivateEffect(s);
                }
            }
            AlignMenuItems();
        }

        public void UnloadContent()
        {
            foreach (var item in Items)
            {
                item.Image.UnloadContent();
            }
        }

        public void Update(GameTime gameTime)
        {
            if (Axis == "X")
            {
                if (InputManager.Instance.KeyPressed(Keys.Right))
                    _itemNumber++;
                else if (InputManager.Instance.KeyPressed(Keys.Left))
                    _itemNumber--;
            }
            else if(Axis == "Y")
            {
                if (InputManager.Instance.KeyPressed(Keys.Down))
                    _itemNumber++;
                else if (InputManager.Instance.KeyPressed(Keys.Up))
                    _itemNumber--;
            }

            if (_itemNumber < 0)
                _itemNumber = 0;
            else if (_itemNumber >= Items.Count)
                _itemNumber = Items.Count - 1;

            for (int i = 0; i < Items.Count; i++)
            {
                if (i == _itemNumber)
                    Items[i].Image.IsActive = true;
                else
                    Items[i].Image.IsActive = false;

                Items[i].Image.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var item in Items)
            {
               item.Image.Draw(spriteBatch); 
            }
        }
    }
}
