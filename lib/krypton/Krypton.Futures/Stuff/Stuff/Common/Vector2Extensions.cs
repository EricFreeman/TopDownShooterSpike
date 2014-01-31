// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Vector2Extensions.cs" company="Christopher Harris">
//   2011 Christopher Harris
// </copyright>
// <summary>
//   AssemblyInfo.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Futures.Common
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// A set of extension methods for Vector2
    /// </summary>
    public static class Vector2Extensions
    {
        /// <summary>
        /// Rotates a vector Clockwise 90 degrees
        /// </summary>
        /// <param name="v">The vector.</param>
        /// <returns>
        /// The rotated vector.
        /// </returns>
        public static Vector2 Clockwise(this Vector2 v)
        {
            return new Vector2(v.Y, -v.X);
        }

        /// <summary>
        /// Rotates a vector CounterClockwise 90 degrees
        /// </summary>
        /// <param name="v">The vector.</param>
        /// <returns>
        /// The rotated vector.
        /// </returns>
        public static Vector2 CounterClockwise(this Vector2 v)
        {
            return new Vector2(-v.Y, v.X);
        }

        /// <summary>
        /// Gets the normalized version of the vector
        /// </summary>
        /// <param name="v">The vector.</param>
        /// <returns>
        /// The normalized vector
        /// </returns>
        public static Vector2 Unit(this Vector2 v)
        {
            v.Normalize();

            return v;
        }
    }
}