using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TopDownShooterSpike.Simulation;

namespace TopDownShooterSpike.Graphics
{
    public interface ITileProvider
    {
        void GetTexture(ref Tile tile, out Texture2D texture);
    }

    public class DefaultTileProvider : ITileProvider
    {
        private readonly ContentManager _contentManager;
        private readonly Dictionary<Tile, string> _textureMaps = new Dictionary<Tile, string>
        {
            // add paths for tile images here
            {Tile.Wall, string.Empty}
        };

        private const string _default = "gfx/Tiles/1";


        public DefaultTileProvider(ContentManager contentManager)
        {
            _contentManager = contentManager;
        }

        public void GetTexture(ref Tile tile, out Texture2D texture)
        {
            string path;

            if (_textureMaps.TryGetValue(tile, out path))
                path = _default;

            texture = _contentManager.Load<Texture2D>(path);
        }
    }
}