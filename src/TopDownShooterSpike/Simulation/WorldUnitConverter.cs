namespace TopDownShooterSpike.Simulation
{
    public class WorldUnitConverter
    {
        public WorldUnitConverter() : this(16) { }

        public WorldUnitConverter(float unitsPerMeter)
        {
            UnitsPerMeter = unitsPerMeter;
        }

        public float ToMeters(float units)
        {
            return units / UnitsPerMeter;
        }

        public float FromMeters(float meters)
        {
            return meters * UnitsPerMeter;
        }

        public float UnitsPerMeter { get; set; }
    }
}