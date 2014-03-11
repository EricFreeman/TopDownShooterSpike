using Microsoft.Xna.Framework;

namespace TopDownShooterSpike.Simulation
{
    public struct Kilogram
    {
        private const float POUNDS_PER_KILO = 2.20462f;
        private float _value;

        public Kilogram(float value)
        {
            _value = value;
        }

        public static implicit operator float(Kilogram kilo)
        {
            return kilo.Value;
        }

        public static Kilogram FromPounds(float pounds)
        {
            return new Kilogram(pounds / POUNDS_PER_KILO);
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