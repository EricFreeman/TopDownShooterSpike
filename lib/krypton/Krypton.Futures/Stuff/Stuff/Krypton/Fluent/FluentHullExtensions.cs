// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FluentHullExtensions.cs" company="Christopher Harris">
//   2011 Christopher Harris
// </copyright>
// <summary>
//   A set of fluent Hull extension methods
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Futures.Krypton.Fluent
{
    using Futures.Krypton;

    using Microsoft.Xna.Framework;

    /// <summary>
    /// A set of fluent Hull extension methods
    /// </summary>
    public static class FluentHullExtensions
    {
        /// <summary>
        /// Sets the hull's position
        /// </summary>
        /// <param name="hull">The hull.</param>
        /// <param name="position">The position.</param>
        /// <returns>
        /// The same hull.
        /// </returns>
        public static Hull Position(this Hull hull, Vector2 position)
        {
            hull.Position = position;
            return hull;
        }

        /// <summary>
        /// Sets the hull's position
        /// </summary>
        /// <param name="hull">The hull.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns>
        /// The same hull.
        /// </returns>
        public static Hull Position(this Hull hull, float x, float y)
        {
            hull.Position = new Vector2(x, y);
            return hull;
        }

        /// <summary>
        /// Sets the hull's position
        /// </summary>
        /// <param name="hull">The hull.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns>
        /// The same hull.
        /// </returns>
        public static Hull Position(this Hull hull, double x, double y)
        {
            hull.Position = new Vector2((float)x, (float)y);
            return hull;
        }

        /// <summary>
        /// Sets the hull's angle.
        /// </summary>
        /// <param name="hull">The hull.</param>
        /// <param name="angle">The angle.</param>
        /// <returns>
        /// The same hull.
        /// </returns>
        public static Hull Angle(this Hull hull, float angle)
        {
            hull.Angle = angle;
            return hull;
        }

        /// <summary>
        /// Sets the hull's scale.
        /// </summary>
        /// <param name="hull">The hull.</param>
        /// <param name="scale">The scale.</param>
        /// <returns>
        /// The hull
        /// </returns>
        public static Hull Scale(this Hull hull, Vector2 scale)
        {
            hull.Scale = scale;
            return hull;
        }
    }
}