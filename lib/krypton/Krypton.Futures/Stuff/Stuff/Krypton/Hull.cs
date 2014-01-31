// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Hull.cs" company="Christopher Harris">
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

    using Futures.Common;
    using Futures.Krypton.Design;

    using Microsoft.Xna.Framework;

    /// <summary>
    /// A Krypton Hull
    /// </summary>
    public class Hull : IHull
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Hull"/> class.
        /// </summary>
        /// <param name="points">The points.</param>
        public Hull(IList<Vector2> points)
        {
            var numVertices = points.Count * 2;
            var numTris = numVertices - 2;
            var numIndicies = numTris * 3;

            this.vertices = new HullVertex[numVertices];
            this.indices = new int[numIndicies];
            this.scale = Vector2.One;

            for (var i = 0; i < points.Count; i++)
            {
                var p1 = points[i];
                var p2 = points[(i + 1) % points.Count];

                var normal = (p2 - p1).Clockwise();

                normal.Normalize();

                this.vertices[i * 2] = new HullVertex(p1, normal, new Color(0, 0, 0, 0.1f));
                this.vertices[(i * 2) + 1] = new HullVertex(p2, normal, new Color(0, 0, 0, 0.1f));
            }

            for (var i = 0; i < numTris; i++)
            {
                this.indices[i * 3] = 0;
                this.indices[(i * 3) + 1] = i + 1;
                this.indices[(i * 3) + 2] = i + 2;
            }

            this.RadiusSquared = points.Max(x => x.LengthSquared());
        }

        public float RadiusSquared { get; private set; }

        private HullVertex[] vertices;

        /// <summary>
        /// Gets the hull vertices.
        /// </summary>
        public HullVertex[] Vertices
        {
            get
            {
                return this.vertices;
            }
            private set
            {
                this.vertices = value;
            }
        }

        private int[] indices;

        /// <summary>
        /// Gets the hull vertices.
        /// </summary>
        public int[] Indices
        {
            get
            {
                return this.indices;
            }
            private set
            {
                this.indices = value;
            }
        }

        /// <summary>
        /// Draws the hull.
        /// </summary>
        /// <param name="drawBuffer">The Lightmap Draw Buffer</param>
        public void Draw(ILightmapDrawBuffer drawBuffer)
        {
            // Create the matrices (3X speed boost versus prior version)
            this.cos = (float)Math.Cos(this.angle);
            this.sin = (float)Math.Sin(this.angle);

            // vertexMatrix = scale * rotation * translation;
            this.vertexMatrix.M11 = this.scale.X * this.cos;
            this.vertexMatrix.M12 = this.scale.X * this.sin;
            this.vertexMatrix.M21 = this.scale.Y * -this.sin;
            this.vertexMatrix.M22 = this.scale.Y * this.cos;
            this.vertexMatrix.M41 = this.position.X;
            this.vertexMatrix.M42 = this.position.Y;

            // normalMatrix = scaleInv * rotation;
            this.normalMatrix.M11 = (1f / this.scale.X) * this.cos;
            this.normalMatrix.M12 = (1f / this.scale.X) * this.sin;
            this.normalMatrix.M21 = (1f / this.scale.Y) * -this.sin;
            this.normalMatrix.M22 = (1f / this.scale.Y) * this.cos;

            drawBuffer.SetStartVertex();

            // Add the vertices to the buffer

            var hullVerticesLength = this.Vertices.Length;
            for (var i = 0; i < hullVerticesLength; i++)
            {
                // Transform the vertices to world coordinates
                this.point = this.Vertices[i];

                Vector2.Transform(ref this.point.Position, ref this.vertexMatrix, out this.hullVertex.Position);
                Vector2.TransformNormal(ref this.point.Normal, ref this.normalMatrix, out this.hullVertex.Normal);

                this.hullVertex.Color = ShadowBlack;

                drawBuffer.AddVertex(this.hullVertex);
            }

            var hullIndicesLength = this.Indices.Length;
            for (var i = 0; i < hullIndicesLength; i++)
            {
                drawBuffer.AddIndex(this.Indices[i]);
            }
        }

        private float angle;

        /// <summary>
        /// Gets or sets Angle.
        /// </summary>
        public float Angle
        {
            get
            {
                return this.angle;
            }
            set
            {
                this.angle = value;
            }
        }

        private Vector2 scale;

        /// <summary>
        /// Gets or sets Scale.
        /// </summary>
        public Vector2 Scale
        {
            get
            {
                return this.scale;
            }
            set
            {
                this.scale = value;
            }
        }

        private Vector2 position;

        private static readonly Color ShadowBlack = new Color(0, 0, 0, 0);

        private Matrix normalMatrix = Matrix.Identity;

        private Matrix vertexMatrix = Matrix.Identity;

        private float cos;

        private float sin;

        private HullVertex hullVertex;

        private HullVertex point;

        /// <summary>
        /// Gets or sets Position.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return this.position;
            }
            set
            {
                this.position = value;
            }
        }
    }
}