using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TopDownShooterSpike.Effects;
using TopDownShooterSpike.Managers;

namespace TopDownShooterSpike
{
    public class Image
    {
        #region Properties

        public float Alpha, Rotation;
        public string Text, FontName, Path;
        public Vector2 Position, Scale;
        public Rectangle SourceRect;
        public Vector2 PubOffset;

        [XmlIgnore]
        public Texture2D Texture;
        private Vector2 _origin;
        private ContentManager _manager;
        private RenderTarget2D _renderTarget;

        private Dictionary<string, ImageEffect> _effectList;
        public string Effects;
        public bool IsActive;

        public FadeEffect FadeEffect;

        #endregion

        #region Constructor

        public Image()
        {
            Text = Path = Effects = String.Empty;
            FontName = "Fonts/SampleFont";
            Position = PubOffset = Vector2.Zero;
            Scale = Vector2.One;
            Alpha = 1.0f;
            SourceRect = Rectangle.Empty;
            _effectList = new Dictionary<string, ImageEffect>();
        }

        #endregion

        #region Effects

        void SetEffect<T>(ref T effect)
        {
            if (effect == null)
                effect = (T)Activator.CreateInstance(typeof(T));
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

            if (Effects != string.Empty)
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

        #endregion

        #region Collision

        // For all their help with writing this code, I'd like to thank Stack Overflow, boxed wine, and...well, that's about it
        public bool CollidesWith(Image otherImage)
        {
            var otherX = otherImage.Position.X;
            var otherY = otherImage.Position.Y;
            var otherRot = otherImage.Rotation;
            var otherTexture = otherImage.Texture;
            var otherWidth = otherTexture.Width;
            var otherHeight = otherTexture.Height;

            var otherTextureData = new Color[otherWidth * otherHeight];
            var thisTextureData = new Color[Texture.Width * Texture.Height];
            otherTexture.GetData(otherTextureData);
            Texture.GetData(thisTextureData);

            var otherTransform =
                Matrix.CreateTranslation(new Vector3(-1 * otherImage.PubOffset, 0.0f)) *    //offset
                Matrix.CreateRotationZ(otherRot) *                                          //rotation
                Matrix.CreateTranslation(new Vector3(otherX, otherY, 0f));                  //position

            var otherRectangle =
                 new Rectangle((int)(otherX - otherImage.PubOffset.X), (int)(otherY - otherImage.PubOffset.Y), otherWidth, otherHeight);

            var thisTransform =
                    Matrix.CreateTranslation(new Vector3(-1 * PubOffset, 0.0f)) *
                    Matrix.CreateRotationZ(Rotation) *
                    Matrix.CreateTranslation(new Vector3(Position, 0.0f));

            var thisRectangle = CalculateBoundingRectangle(
                         new Rectangle(0, 0, 16, 64),
                         thisTransform);

            // do simple rectangle check before doing per pixel to save time
            if (otherRectangle.Intersects(thisRectangle))
            {
                // if simple collision passed, now check on a per pixel basis
                if (IntersectPixels(otherTransform, otherTexture.Width,
                                    otherTexture.Height, otherTextureData,
                                    thisTransform, Texture.Width,
                                    Texture.Height, thisTextureData))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IntersectPixels(Matrix transformA, int widthA, int heightA, Color[] dataA,
                                           Matrix transformB, int widthB, int heightB, Color[] dataB)
        {
            // Calculate a matrix which transforms from A's local space into
            // world space and then into B's local space
            var transformAToB = transformA * Matrix.Invert(transformB);

            // When a point moves in A's local space, it moves in B's local space with a
            // fixed direction and distance proportional to the movement in A.
            // This algorithm steps through A one pixel at a time along A's X and Y axes
            // Calculate the analogous steps in B:
            var stepX = Vector2.TransformNormal(Vector2.UnitX, transformAToB);
            var stepY = Vector2.TransformNormal(Vector2.UnitY, transformAToB);

            // Calculate the top left corner of A in B's local space
            // This variable will be reused to keep track of the start of each row
            var yPosInB = Vector2.Transform(Vector2.Zero, transformAToB);

            // For each row of pixels in A
            for (int yA = 0; yA < heightA; yA++)
            {
                // Start at the beginning of the row
                var posInB = yPosInB;

                // For each pixel in this row
                for (int xA = 0; xA < widthA; xA++)
                {
                    // Round to the nearest pixel
                    var xB = (int)Math.Round(posInB.X);
                    var yB = (int)Math.Round(posInB.Y);

                    // If the pixel lies within the bounds of B
                    if (0 <= xB && xB < widthB &&
                        0 <= yB && yB < heightB)
                    {
                        // Get the colors of the overlapping pixels
                        Color colorA = dataA[xA + yA * widthA];
                        Color colorB = dataB[xB + yB * widthB];

                        // If both pixels are not completely transparent,
                        if (colorA.A != 0 && colorB.A != 0)
                        {
                            // then an intersection has been found
                            return true;
                        }
                    }

                    // Move to the next pixel in the row
                    posInB += stepX;
                }

                // Move to the next row
                yPosInB += stepY;
            }

            // No intersection found
            return false;
        }

        public static Rectangle CalculateBoundingRectangle(Rectangle rectangle, Matrix transform)
        {
            // Get all four corners in local space
            var leftTop = new Vector2(rectangle.Left, rectangle.Top);
            var rightTop = new Vector2(rectangle.Right, rectangle.Top);
            var leftBottom = new Vector2(rectangle.Left, rectangle.Bottom);
            var rightBottom = new Vector2(rectangle.Right, rectangle.Bottom);

            // Transform all four corners into work space
            Vector2.Transform(ref leftTop, ref transform, out leftTop);
            Vector2.Transform(ref rightTop, ref transform, out rightTop);
            Vector2.Transform(ref leftBottom, ref transform, out leftBottom);
            Vector2.Transform(ref rightBottom, ref transform, out rightBottom);

            // Find the minimum and maximum extents of the rectangle in world space
            var min = Vector2.Min(Vector2.Min(leftTop, rightTop), Vector2.Min(leftBottom, rightBottom));
            var max = Vector2.Max(Vector2.Max(leftTop, rightTop), Vector2.Max(leftBottom, rightBottom));

            // Return that as a rectangle
            return new Rectangle((int)min.X, (int)min.Y, (int)(max.X - min.X), (int)(max.Y - min.Y));
        }

        #endregion

        #region Hooks

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

            spriteBatch.Draw(Texture, Position + _origin, null, Color.White * Alpha, Rotation, _origin + PubOffset, Scale, SpriteEffects.None, 0.0f);
        }

        #endregion
    }
}
