using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TopDownShooterSpike.Graphics
{
    public abstract class RenderObject
    {
        public virtual bool Visible { get; set; }
        public bool Lit { get; set; }
        public Transform2D Transform { get; set; }

        public abstract void Draw(RenderContext context);
    }

    public class SpriteRenderObject : RenderObject
    {
        private Texture2D _sprite;

        public SpriteRenderObject()
        {
            SourceRectangle = null;
            Color = Color.White;
        }

        public override void Draw(RenderContext context)
        {
            context.SpriteBatch.Draw(Sprite, 
                                     Transform.Position, 
                                     SourceRectangle, 
                                     Color, 
                                     Transform.Rotation, 
                                     Origin, 
                                     Transform.Scale, 
                                     SpriteEffects.None, 
                                     Transform.ZIndex);
        }

        public Texture2D Sprite
        {
            get { return _sprite; }
            set
            {
                
                _sprite = value;

                if(_sprite != null)
                    Origin = new Vector2(_sprite.Width / 2.0f, _sprite.Height / 2.0f);
                
            }
        }

        public Rectangle? SourceRectangle { get; set; }
        public Vector2 Origin { get; set; }
        public Color Color { get; set; }

        public override bool Visible
        {
            get { return base.Visible && _sprite != null; }
            set { base.Visible = value; }
        }
    }
}
