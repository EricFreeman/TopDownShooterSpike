// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RandomExtensions.cs" company="Christopher Harris">
//   2011 Christopher Harris
// </copyright>
// <summary>
//   A set of extension methods for <see cref="Random" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Futures.Common
{
    using System;

    using Microsoft.Xna.Framework;

    /// <summary>
    /// A set of extension methods for <see cref="Random"/>
    /// </summary>
    public static class RandomExtensions
    {
        /// <summary>
        /// Gets a random vector between the <param name="min">minimum</param> and <param name="max">maximum</param>
        /// </summary>
        /// <param name="random">The random.</param>
        /// <param name="min">The minimum (-x and -y).</param>
        /// <param name="max">The maximum (+x and +y).</param>
        /// <returns>
        /// A random vector
        /// </returns>
        public static Vector2 NextVector(this Random random, float min, float max)
        {
            var size = max - min;
            return new Vector2((float)(random.NextDouble() * size) + min, (float)(random.NextDouble() * size) + min);
        }

        /// <summary>
        /// Gets a random angle between +<see cref="MathHelper.Pi"/> and -<see cref="MathHelper.Pi"/>
        /// </summary>
        /// <param name="random">The random.</param>
        /// <returns>
        /// A random angle
        /// </returns>
        public static float NextAngle(this Random random)
        {
            return ((float)random.NextDouble() * MathHelper.TwoPi) - MathHelper.Pi;
        }
    }
}