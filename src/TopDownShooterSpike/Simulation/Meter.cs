namespace TopDownShooterSpike.Simulation
{
    public struct Meter
    {
        private float _value;
        private const float INCHES_PER_METER = 39.3701f;

        public Meter(float value)
        {
            _value = value;
        }

        public static implicit operator float(Meter meter)
        {
            return meter.Value;
        }

        public static Meter FromInches(float inches)
        {
            return new Meter(inches / INCHES_PER_METER);
        }

        public float Centimeters
        {
            get { return _value*100; }
            set { _value = value/100.0f; }
        }

        public float Value 
        { 
            get { return _value; } 
            set { _value = value; }
        }

    }
}