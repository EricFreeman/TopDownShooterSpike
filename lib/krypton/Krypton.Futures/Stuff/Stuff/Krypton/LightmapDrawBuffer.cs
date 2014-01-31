// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LightmapDrawBuffer.cs" company="Christopher Harris">
//   2011 Christopher Harris
// </copyright>
// <summary>
//   AssemblyInfo.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Futures.Krypton
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Futures.Krypton.Design;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The Default LightMapper Draw Bufferr
    /// </summary>
    public class LightmapDrawBuffer : ILightmapDrawBuffer
    {
        /// <summary>
        /// The graphics device
        /// </summary>
        private readonly GraphicsDevice device;

        /// <summary>
        /// The hull vertices
        /// </summary>
        private readonly List<HullVertex> vertices;

        // /// <summary>
        // /// The hull indices
        // /// </summary>
        // private readonly List<int> indices;

        /// <summary>
        /// A unit quad
        /// </summary>
        private static readonly VertexPositionTexture[] UnitQuad;

        /// <summary>
        /// The Clipped FOV Vertices
        /// </summary>
        private VertexPositionColorTexture[] clippedFovVertices;

        private int[] indicesArray = new int[1024 * 1024];

        private int numIndicies;

        private int startVertex;

        /// <summary>
        /// Initializes static members of the <see cref="LightmapDrawBuffer"/> class.
        /// </summary>
        static LightmapDrawBuffer()
        {
            UnitQuad = new[]
                {
                    new VertexPositionTexture(new Vector3(-1, +1, 0), new Vector2(0, 0)),
                    new VertexPositionTexture(new Vector3(+1, +1, 0), new Vector2(1, 0)),
                    new VertexPositionTexture(new Vector3(-1, -1, 0), new Vector2(0, 1)),
                    new VertexPositionTexture(new Vector3(+1, -1, 0), new Vector2(1, 1)),
                };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LightmapDrawBuffer"/> class.
        /// </summary>
        /// <param name="device">The graphics device.</param>
        public LightmapDrawBuffer(GraphicsDevice device)
        {
            this.device = device;

            // this.indices = new List<int>();
            this.vertices = new List<HullVertex>();
        }

        /// <summary>
        /// Draws the contents of the buffer.
        /// </summary>
        public void Draw()
        {
            if (this.numIndicies < 3)
            {
                return;
            }

            this.device.DrawUserIndexedPrimitives(
                PrimitiveType.TriangleList,
                this.vertices.ToArray(),
                0,
                this.vertices.Count,
                this.indicesArray,
                0,
                this.numIndicies / 3);
        }

        /// <summary>
        /// Clears the buffer.
        /// </summary>
        public void Clear()
        {
            // this.indices.Clear();
            this.vertices.Clear();
            this.numIndicies = 0;
        }

        /// <summary>
        /// Draws a unit quad. (-1, -1) to (+1, +1)
        /// </summary>
        public void DrawUnitQuad()
        {
            this.device.DrawUserPrimitives(PrimitiveType.TriangleStrip, UnitQuad, 0, 2);
        }

        /// <summary>
        /// Draws a "quad", clipped to show only what portions are inside the fov
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="rotation">The rotation.</param>
        /// <param name="size">The size (width and/or height).</param>
        /// <param name="color">The color.</param>
        /// <param name="fov">The field of view.</param>
        public void DrawClippedFov(Vector2 position, float rotation, float size, Color color, float fov)
        {
            if (fov <= 0)
            {
                return;
            }

            if (fov >= MathHelper.TwoPi)
            {
                this.DrawSquareQuad(position, rotation, size, color);
                return;
            }

            fov = MathHelper.Clamp(fov, 0, MathHelper.TwoPi);

            var ccw = ClampToUnitSquare(fov / 2);
            var cw = ClampToUnitSquare(-fov / 2);

            var texCcw = new Vector2(ccw.X + 1, -ccw.Y + 1) / 2;
            var texCw = new Vector2(cw.X + 1, -cw.Y + 1) / 2;

            this.clippedFovVertices = new[]
                {
                    new VertexPositionColorTexture(Vector3.Zero, color, new Vector2(0.5f, 0.5f)),
                    new VertexPositionColorTexture(new Vector3(ccw, 0), color, texCcw),
                    new VertexPositionColorTexture(new Vector3(-1, +1, 0), color, new Vector2(0, 0)),
                    new VertexPositionColorTexture(new Vector3(+1, +1, 0), color, new Vector2(1, 0)),
                    new VertexPositionColorTexture(new Vector3(+1, -1, 0), color, new Vector2(1, 1)),
                    new VertexPositionColorTexture(new Vector3(-1, -1, 0), color, new Vector2(0, 1)),
                    new VertexPositionColorTexture(new Vector3(cw, 0), color, texCw),
                };

            var matrix =
                Matrix.CreateRotationZ(rotation) *
                Matrix.CreateScale(size / 2) *
                Matrix.CreateTranslation(new Vector3(position, 0));

            for (var i = 0; i < this.clippedFovVertices.Length; i++)
            {
                var vertex = this.clippedFovVertices[i];

                Vector3.Transform(ref vertex.Position, ref matrix, out vertex.Position);

                this.clippedFovVertices[i] = vertex;
            }

            var clippedFovIndices = GetClippedFovIndicies(fov);

            this.device.DrawUserIndexedPrimitives(
                PrimitiveType.TriangleList,
                this.clippedFovVertices,
                0,
                this.clippedFovVertices.Length,
                clippedFovIndices,
                0,
                clippedFovIndices.Length / 3);
        }

        public void AddIndex(int index)
        {
            this.indicesArray[this.numIndicies++] = index + this.startVertex;
        }

        public void AddVertex(HullVertex hullVertex)
        {
            this.vertices.Add(hullVertex);
        }

        public void SetStartVertex()
        {
            this.startVertex = this.vertices.Count;
        }

        /// <summary>
        /// Gets indices for a clipped FOV quad.
        /// </summary>
        /// <param name="fov">The fov.</param>
        /// <returns>
        /// Indicies for the clipped fov quad.
        /// </returns>
        private static int[] GetClippedFovIndicies(float fov)
        {
            int[] clippedFovIndices;
            if (fov <= MathHelper.Pi / 2)
            {
                clippedFovIndices = new[] { 0, 1, 6, };
            }
            else if (fov <= 3 * MathHelper.Pi / 2)
            {
                clippedFovIndices = new[] { 0, 1, 3, 0, 3, 4, 0, 4, 6, };
            }
            else
            {
                clippedFovIndices = new[] { 0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 5, 0, 5, 6, };
            }
            
            return clippedFovIndices;
        }

        /// <summary>
        /// Gets a vector that has been clamped to a a unit square.
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <returns>
        /// A vector that reaches only to the edge of a unit square.
        /// </returns>
        private static Vector2 ClampToUnitSquare(float angle)
        {
            var cos = Math.Cos(angle);
            var sin = Math.Sin(angle);

            var absMax = Math.Max(Math.Abs(cos), Math.Abs(sin));

            return new Vector2((float)(cos / absMax), (float)(sin / absMax));
        }

        /// <summary>
        /// Draws a square quad.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="rotation">The rotation.</param>
        /// <param name="size">The size.</param>
        /// <param name="color">The color.</param>
        private void DrawSquareQuad(Vector2 position, float rotation, float size, Color color)
        {
            size = (size / 2) * (float)Math.Sqrt(2); // sqrt(x^2 + y^2); where (x = size / 2) and (y = size/2)

            rotation += (float)Math.PI / 4;

            var cos = (float)Math.Cos(rotation) * size;
            var sin = (float)Math.Sin(rotation) * size;

            var v1 = new Vector3(+cos + position.X, +sin + position.Y, 0);
            var v2 = new Vector3(-sin + position.X, +cos + position.Y, 0);
            var v3 = new Vector3(-cos + position.X, -sin + position.Y, 0);
            var v4 = new Vector3(+sin + position.X, -cos + position.Y, 0);

            var quad = new[]
                {
                    new VertexPositionColorTexture(v2, color, new Vector2(0, 0)),
                    new VertexPositionColorTexture(v1, color, new Vector2(1, 0)),
                    new VertexPositionColorTexture(v3, color, new Vector2(0, 1)),
                    new VertexPositionColorTexture(v4, color, new Vector2(1, 1)),
                };

            this.device.DrawUserPrimitives(PrimitiveType.TriangleStrip, quad, 0, 2);
        }
    }
}