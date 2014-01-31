// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HullFactory.cs" company="Christopher Harris">
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

    /// <summary>
    /// The Default Hull Factory
    /// </summary>
    public static class HullFactory
    {
        /// <summary>
        /// Create a circlular hull
        /// </summary>
        /// <param name="radius">The radius.</param>
        /// <param name="numSides">The num sides.</param>
        /// <returns>
        /// A Circular ShadowHull
        /// </returns>
        public static Hull CreateCircle(float radius, int numSides)
        {
            var vertices = new Vector2[numSides];

            var angle = (Math.PI * 2) / numSides;

            for (var i = 0; i < numSides; i++)
            {
                vertices[i] = new Vector2((float)Math.Cos(i * angle) * radius, (float)Math.Sin(i * angle) * radius);
            }

            return new Hull(vertices);
        }

        /// <summary>
        /// Creates a rectangle hull.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns>
        /// A new rectangle hull.
        /// </returns>
        public static Hull CreateRectangle(float width, float height)
        {
            return
                new Hull(
                    new[]
                        {
                            new Vector2(+width, +height),
                            new Vector2(-width, +height),
                            new Vector2(-width, -height),
                            new Vector2(+width, -height),
                        });
        }
    }
}