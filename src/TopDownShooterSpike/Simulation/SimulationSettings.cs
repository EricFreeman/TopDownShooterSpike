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
}