using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;

namespace TopDownShooterSpike.Graphics
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Transform2D
    {
        [FieldOffset(0)]
        public Vector2 Position;

        [FieldOffset(8)]
        public Vector2 Scale;

        [FieldOffset(16)]
        public float ZIndex;

        [FieldOffset(20)]
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