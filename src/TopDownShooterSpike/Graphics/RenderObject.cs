using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TopDownShooterSpike.Graphics
{
    public abstract class RenderObject
    {
        public bool Visible { get; set; }
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

    }

    [StructLayout(LayoutKind.Explicit)]
    public struct Transform2D
    {
        [FieldOffset(0)]
        public Vector2 Position;

        [FieldOffset(8)]
        public Vector2 Scale;

        [FieldOffset(16)]
        public float ZIndex;

        [FieldOffset(17)]
        public float Rotation;

        public static readonly Transform2D Zero = new Transform2D()
        {
            Position =  Vector2.Zero,
            Scale = Vector2.One,
            Rotation =  0,
            ZIndex = 0
        };

        public float WrappedRotation()
        {
            return MathHelper.WrapAngle(Rotation);
        }

        public Matrix TranslationMatrix()
        {
            return Matrix.CreateTranslation(new Vector3(Position, ZIndex));
        }

        public Matrix RotationMatrix()
        {
            return Matrix.CreateRotationZ(Rotation);
        }

        public Matrix ScaleMatrix()
        {
            return Matrix.CreateScale(Scale.X, Scale.Y, 1);
        }

        public Matrix Combine()
        {
            return ScaleMatrix()*RotationMatrix()*TranslationMatrix();
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            //       
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237  
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            // TODO: write your implementation of Equals() here
            return GetHashCode() == obj.GetHashCode();
        }

// override object.GetHashCode
        public override int GetHashCode()
        {
            // TODO: write your implementation of GetHashCode() here
            return Position.GetHashCode()*41;
        }
    }
}
