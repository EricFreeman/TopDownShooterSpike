// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HullVertex.cs" company="Christopher Harris">
//   2011 Christopher Harris
// </copyright>
// <summary>
//   AssemblyInfo.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Futures.Krypton
{
    using System.Diagnostics;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Hull Vertex
    /// </summary>
    [DebuggerDisplay("{Position} {Normal} {Color}")]
    public struct HullVertex : IVertexType
    {
        /// <summary>
        /// The Position
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// The Normal
        /// </summary>
        public Vector2 Normal;

        /// <summary>
        /// The Opacity
        /// </summary>
        public Color Color;

        /// <summary>
        /// Vertex Declaration
        /// </summary>
        private static readonly VertexDeclaration Declaration;

        /// <summary>
        /// Initializes static members of the <see cref="HullVertex"/> struct.
        /// </summary>
        static HullVertex()
        {
            Declaration =
                new VertexDeclaration(
                    new[]
                        {
                            new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 0),
                            new VertexElement(8, VertexElementFormat.Vector2, VertexElementUsage.Normal, 0),
                            new VertexElement(16, VertexElementFormat.Color, VertexElementUsage.Color, 0),
                        });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HullVertex"/> struct.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="normal">The normal.</param>
        public HullVertex(Vector2 position, Vector2 normal)
            : this(position, normal, 1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HullVertex"/> struct.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="normal">The normal.</param>
        /// <param name="opacity">The opacity.</param>
        public HullVertex(Vector2 position, Vector2 normal, float opacity)
            : this(position, normal, new Color(0, 0, 0, opacity))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HullVertex"/> struct.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="normal">The normal.</param>
        /// <param name="color">The color.</param>
        public HullVertex(Vector2 position, Vector2 normal, Color color)
        {
            this.Position = position;
            this.Normal = normal;
            this.Color = color;
        }

        /// <summary>
        /// Gets VertexDeclaration.
        /// </summary>
        public VertexDeclaration VertexDeclaration
        {
            get
            {
                return Declaration;
            }
        }
    }
}