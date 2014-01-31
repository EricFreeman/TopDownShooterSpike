// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DrawToTargetExtensions.cs" company="Christopher Harris">
//   2011 Christopher Harris
// </copyright>
// <summary>
//   AssemblyInfo.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Futures.Common
{
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// A set of extension methods to help draw textures to the current render target
    /// </summary>
    public static class DrawToTargetExtensions
    {
        /// <summary>
        /// Draws textures to the render target using a single SpriteBatch call
        /// </summary>
        /// <param name="spriteBatch">The sprite batch.</param>
        /// <param name="blendState">The blend state.</param>
        /// <param name="tex1">Texture 1.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if texture is null
        /// </exception>
        public static void DrawToTarget(this SpriteBatch spriteBatch, BlendState blendState, Texture2D tex1)
        {
            if (tex1 == null)
            {
                throw new ArgumentNullException("tex1");
            }

            spriteBatch.BeginDrawToTarget(blendState);

            spriteBatch.DrawToTarget(tex1);

            spriteBatch.End();
        }

        /// <summary>
        /// Draws textures to the render target using a single SpriteBatch call
        /// </summary>
        /// <param name="spriteBatch">The sprite batch.</param>
        /// <param name="blendState">The blend state.</param>
        /// <param name="tex1">Texture 1.</param>
        /// <param name="tex2">Texture 2.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if any of the textures are null
        /// </exception>
        public static void DrawToTarget(this SpriteBatch spriteBatch, BlendState blendState, Texture2D tex1, Texture2D tex2)
        {
            if (tex1 == null)
            {
                throw new ArgumentNullException("tex1");
            }

            if (tex2 == null)
            {
                throw new ArgumentNullException("tex2");
            }

            spriteBatch.BeginDrawToTarget(blendState);

            spriteBatch.DrawToTarget(tex1);
            spriteBatch.DrawToTarget(tex2);

            spriteBatch.End();
        }

        /// <summary>
        /// Draws textures to the render target using a single SpriteBatch call
        /// </summary>
        /// <param name="spriteBatch">The sprite batch.</param>
        /// <param name="blendState">The blend state.</param>
        /// <param name="tex1">Texture 1.</param>
        /// <param name="tex2">Texture 2.</param>
        /// <param name="tex3">texture 3.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if any of the textures are null
        /// </exception>
        public static void DrawToTarget(this SpriteBatch spriteBatch, BlendState blendState, Texture2D tex1, Texture2D tex2, Texture2D tex3)
        {
            if (tex1 == null)
            {
                throw new ArgumentNullException("tex1");
            }

            if (tex2 == null)
            {
                throw new ArgumentNullException("tex2");
            }

            if (tex3 == null)
            {
                throw new ArgumentNullException("tex3");
            }

            spriteBatch.BeginDrawToTarget(blendState);

            spriteBatch.DrawToTarget(tex1);
            spriteBatch.DrawToTarget(tex2);
            spriteBatch.DrawToTarget(tex3);

            spriteBatch.End();
        }

        /// <summary>
        /// Draws textures to the render target using a single SpriteBatch call
        /// </summary>
        /// <param name="spriteBatch">The sprite batch.</param>
        /// <param name="blendState">The blend state.</param>
        /// <param name="textures">The textures.</param>
        /// <exception cref="ArgumentNullException">
        /// Throw if the textures parameter is null
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Throw if the textures parameter is empty
        /// </exception>
        public static void DrawToTarget(this SpriteBatch spriteBatch, BlendState blendState, params Texture2D[] textures)
        {
            if (textures == null)
            {
                throw new ArgumentNullException("textures");
            }

            if (textures.Length == 0)
            {
                throw new ArgumentException("textures cannot be empty", "textures");
            }

            spriteBatch.BeginDrawToTarget(blendState);

            foreach (var texture in textures)
            {
                spriteBatch.DrawToTarget(texture);
            }

            spriteBatch.End();
        }

        /// <summary>
        /// Draws the texture to the current render target using the SpriteBatch
        /// </summary>
        /// <param name="spriteBatch">The sprite batch.</param>
        /// <param name="texture">The texture.</param>
        private static void DrawToTarget(this SpriteBatch spriteBatch, Texture2D texture)
        {
            spriteBatch.Draw(texture, spriteBatch.GraphicsDevice.Viewport.Bounds, Color.White);
        }

        /// <summary>
        /// Begin's the SpriteBatch with appropriate settings
        /// </summary>
        /// <param name="spriteBatch">The sprite batch.</param>
        /// <param name="blendState">The blend state.</param>
        private static void BeginDrawToTarget(this SpriteBatch spriteBatch, BlendState blendState)
        {
            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                blendState,
                SamplerState.LinearClamp,
                DepthStencilState.None,
                RasterizerState.CullNone,
                null,
                Matrix.Identity);
        }
    }
}