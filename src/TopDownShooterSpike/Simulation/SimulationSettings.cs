namespace TopDownShooterSpike.Simulation
{
    public class SimulationSettings
    {
        private readonly WorldUnitConverter _worldUnitConverter;

        public SimulationSettings()
        {
            TileDimension = new Meter(1);
            _worldUnitConverter = new WorldUnitConverter(32);
        }

        public float TileDimension { get; set; }

        public WorldUnitConverter WorldUnitConverter
        {
            get { return _worldUnitConverter; }
        }

        public float TileDimensionAsWorldUnits
        {
            get { return _worldUnitConverter.FromMeters(TileDimension); }
        }
    }

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