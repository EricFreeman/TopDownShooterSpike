namespace TopDownShooterSpike.Graphics
{
    public abstract class RenderObject
    {
        public bool Visible { get; set; }
        public bool Lit { get; set; }

        public abstract void Draw(RenderContext context);
    }
}
