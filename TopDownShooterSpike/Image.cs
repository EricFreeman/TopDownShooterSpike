using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TopDownShooterSpike.Effects;

namespace TopDownShooterSpike
{
    public class Image
    {
        public float Alpha;
        public string Text, FontName, Path;
        public Vector2 Position, Scale;
        public Rectangle SourceRect;

        [XmlIgnore]
        public Texture2D Texture;
        private Vector2 _origin;
        private ContentManager _manager;
        private RenderTarget2D _renderTarget;

        private Dictionary<string, ImageEffect> _effectList;
        public string Effects;
        public bool IsActive;

        public FadeEffect FadeEffect;

        void SetEffect<T>(ref T effect)
        {
            if (effect == null)
                effect = (T) Activator.CreateInstance(typeof (T));
            else
            {
                (effect as ImageEffect).IsActive = true;
                var obj = this;
                (effect as ImageEffect).LoadContent(ref obj);
            }
            _effectList.Add(effect.GetType().Name, (effect as ImageEffect));
        }

        public void ActivateEffect(string effect)
        {
            if (_effectList.ContainsKey(effect))
            {
                _effectList[effect].IsActive = true;
                var obj = this;
                _effectList[effect].LoadContent(ref obj);
            }
        }

        public void DeactivateEffect(string effect)
        {
            if (_effectList.ContainsKey(effect))
            {
                _effectList[effect].IsActive = false;
                _effectList[effect].UnloadContent();
            }
        }

        public void StoreEffects()
        {
            Effects = string.Empty;
            foreach (var effect in _effectList)
            {
                if (effect.Value.IsActive)
                    Effects += effect.Key + ":";
            }

            if(Effects != string.Empty)
                Effects.Remove(Effects.Length - 1);
        }

        public void RestoreEffects()
        {
            foreach (var effect in _effectList)
                DeactivateEffect(effect.Key);

            string[] split = Effects.Split(':');
            foreach (var e in split)
                ActivateEffect(e);
        }

        public Image()
        {
            Text = Path = Effects = String.Empty;
            FontName = "Fonts/SampleFont";
            Position = Vector2.Zero;
            Scale = Vector2.One;
            Alpha = 1.0f;
            SourceRect = Rectangle.Empty;
            _effectList = new Dictionary<string, ImageEffect>();
        }

        public void LoadContent()
        {
            _manager = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");

            if (!string.IsNullOrEmpty(Path))
                Texture = _manager.Load<Texture2D>(Path);

            var dimensions = Vector2.Zero;
            var font = _manager.Load<SpriteFont>(FontName);

            dimensions.X += Texture != null ? Texture.Width : 0;
            dimensions.X += font.MeasureString(Text).X;
            if (Texture != null)
                dimensions.Y += Math.Max(Texture.Height, font.MeasureString(Text).Y);
            else
                dimensions.Y = font.MeasureString(Text).Y;

            if(SourceRect == Rectangle.Empty)
                SourceRect = new Rectangle(0, 0, (int)dimensions.X, (int)dimensions.Y);

            _renderTarget = new RenderTarget2D(ScreenManager.Instance.GraphicsDevice, (int)dimensions.X, (int)dimensions.Y);
            ScreenManager.Instance.GraphicsDevice.SetRenderTarget(_renderTarget);
            ScreenManager.Instance.GraphicsDevice.Clear(Color.Transparent);

            ScreenManager.Instance.SpriteBatch.Begin();

            if(Texture != null)
                ScreenManager.Instance.SpriteBatch.Draw(Texture, Vector2.Zero, Color.White);

            ScreenManager.Instance.SpriteBatch.DrawString(font, Text, Vector2.Zero, Color.White);
            ScreenManager.Instance.SpriteBatch.End();

            Texture = _renderTarget;
            ScreenManager.Instance.GraphicsDevice.SetRenderTarget(null);

            SetEffect(ref FadeEffect);

            if (Effects != string.Empty)
            {
                string[] split = Effects.Split(':');
                foreach (var item in split)
                {
                    ActivateEffect(item);
                }
            }
        }

        public void UnloadContent()
        {
            _manager.Unload();
            foreach (var imageEffect in _effectList)
            {
                DeactivateEffect(imageEffect.Key);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var imageEffect in _effectList)
            {
                if(imageEffect.Value.IsActive)
                    imageEffect.Value.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _origin = new Vector2(SourceRect.Width/2f, SourceRect.Height/2f);

            spriteBatch.Draw(Texture, Position + _origin, SourceRect, Color.White * Alpha, 0.0f, _origin, Scale, SpriteEffects.None, 0.0f);
        }
    }
}
