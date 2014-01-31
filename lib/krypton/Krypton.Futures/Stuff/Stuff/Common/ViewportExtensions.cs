namespace Futures.Common
{
    using System.Linq;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public static class ViewportExtensions
    {
        /// <summary>
        /// Gets a pixel-space rectangle which contains the light passed in
        /// </summary>
        /// <param name="light">The light used to create the rectangle</param>
        /// <param name="matrix">the WorldViewProjection matrix being used to render</param>
        /// <param name="targetSize">The rendertarget's size</param>
        /// <returns></returns>
        public static Rectangle ScissorRectCreateForLight(Vector2 min, Vector2 max, Matrix matrix, Vector2 targetSize)
        {
            min = VectorToPixel(min, matrix, targetSize);
            max = VectorToPixel(max, matrix, targetSize);

            var min2 = Vector2.Min(min, max);
            var max2 = Vector2.Max(min, max);

            min = Vector2.Clamp(min2, Vector2.Zero, targetSize);
            max = Vector2.Clamp(max2, Vector2.Zero, targetSize);

            return new Rectangle((int)(min.X), (int)(min.Y), (int)(max.X - min.X), (int)(max.Y - min.Y));
        }

        /// <summary>
        /// Takes a screen-space vector and puts it in to pixel space
        /// </summary>
        /// <param name="v"></param>
        /// <param name="matrix"></param>
        /// <param name="targetSize"></param>
        /// <returns></returns>
        private static Vector2 VectorToPixel(Vector2 v, Matrix matrix, Vector2 targetSize)
        {
            Vector2.Transform(ref v, ref matrix, out v);

            v.X = (1 + v.X) * (targetSize.X / 2f);
            v.Y = (1 - v.Y) * (targetSize.Y / 2f);

            return v;
        }
    }
}