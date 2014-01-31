// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILightmapDrawBuffer.cs" company="Christopher Harris">
//   2011 Christopher Harris
// </copyright>
// <summary>
//   AssemblyInfo.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Futures.Krypton.Design
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// A LightMapperDrawBuffer
    /// </summary>
    public interface ILightmapDrawBuffer
    {
        /// <summary>
        /// Draws the contents of the buffer.
        /// </summary>
        void Draw();

        /// <summary>
        /// Clears the buffer.
        /// </summary>
        void Clear();

        /// <summary>
        /// Draws a unit quad. (-1, -1) to (+1, +1)
        /// </summary>
        void DrawUnitQuad();

        /// <summary>
        /// Draws a "quad", clipped to show only what portions are inside the fov
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="rotation">The rotation.</param>
        /// <param name="size">The size (width and/or height).</param>
        /// <param name="color">The color.</param>
        /// <param name="fov">The field of view.</param>
        void DrawClippedFov(Vector2 position, float rotation, float size, Color color, float fov);

        void AddIndex(int index);

        void AddVertex(HullVertex hullVertex);

        void SetStartVertex();
    }
}