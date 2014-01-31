// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextureFactory.cs" company="Christopher Harris">
//   2011 Christopher Harris
// </copyright>
// <summary>
//   AssemblyInfo.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Futures.Krypton.Factories
{
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// A Light Texture Factory
    /// </summary>
    public static class TextureFactory
    {
        /// <summary>
        /// Creates a point light texture with fov and near plane distance
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="size">The size.</param>
        /// <returns>
        /// A Texture
        /// </returns>
        public static Texture2D CreatePoint(GraphicsDevice device, int size)
        {
            return CreateConic(device, size, MathHelper.TwoPi, 0);
        }

        /// <summary>
        /// Creates a conic light texture with fov
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="size">The size.</param>
        /// <param name="fov">The fov.</param>
        /// <returns>
        /// A Texture
        /// </returns>
        public static Texture2D CreateConic(GraphicsDevice device, int size, float fov)
        {
            return CreateConic(device, size, fov, 0);
        }

        /// <summary>
        /// Creates a conic light texture with fov and near plane distance
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="size">The size.</param>
        /// <param name="fov">The fov.</param>
        /// <param name="nearPlaneDistance">The near plane distance.</param>
        /// <returns>
        /// A Texture
        /// </returns>
        public static Texture2D CreateConic(GraphicsDevice device, int size, float fov, float nearPlaneDistance)
        {
            var data1D = new Color[size * size];

            var halfSize = size / 2f;

            fov /= 2;

            for (var x = 0; x < size; x++)
            {
                for (var y = 0; y < size; y++)
                {
                    var vector = new Vector2(x - halfSize, y - halfSize);

                    var distance = vector.Length();

                    var angle = Math.Abs(Math.Atan2(vector.Y, vector.X));

                    var illumination = 0f;

                    if (distance <= halfSize && distance >= nearPlaneDistance && angle <= fov)
                    {
                        illumination = (halfSize - distance) / halfSize;
                    }

                    data1D[x + (y * size)] = new Color(illumination, illumination, illumination, illumination);
                }
            }

            var tex = new Texture2D(device, size, size);

            tex.SetData(data1D);

            return tex;
        }
    }
}
