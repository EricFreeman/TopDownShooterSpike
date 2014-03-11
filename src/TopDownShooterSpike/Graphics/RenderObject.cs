namespace TopDownShooterSpike.Graphics
{
    public abstract class RenderObject
    {
        protected RenderObject()
        {
            Visible = true;
        }

        public Transform2D Transform;
        public virtual bool Visible { get; set; }
        public bool Lit { get; set; }
        public abstract void Draw(RenderContext context);
    }
}
