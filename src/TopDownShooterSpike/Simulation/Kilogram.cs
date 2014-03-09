using Microsoft.Xna.Framework;

namespace TopDownShooterSpike.Simulation
{
    public struct Kilogram
    {
        private float _value;

        public Kilogram(float value)
        {
            _value = value;
        }

        public static implicit operator float(Kilogram kilo)
        {
            return kilo.Value;
        }

        public float Gram
        {
            get { return _value*1000; }
            set { _value = value/1000.0f; }
        }

        public float Value
        {
            get { return _value; }
            set { _value = MathHelper.Max(value, 0); }
        }
    }
}