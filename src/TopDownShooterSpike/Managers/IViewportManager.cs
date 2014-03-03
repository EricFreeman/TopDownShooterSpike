using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TopDownShooterSpike.Managers
{
    public interface IViewportManager
    {
        void SetView(Viewport viewport);
    }

    public class ViewportManager : IViewportManager
    {
        private readonly GraphicsDeviceManager _gdm;
        private readonly GraphicsDevice _graphicsDevice;

        public ViewportManager(GraphicsDeviceManager gdm)
        {
            _gdm = gdm;
            _graphicsDevice = gdm.GraphicsDevice;
        }

        public void SetView(Viewport viewport)
        {
            _graphicsDevice.Viewport = viewport;

            _gdm.PreferredBackBufferHeight = viewport.Height;
            _gdm.PreferredBackBufferWidth = viewport.Width;
            _gdm.ApplyChanges();
        }
    }
}