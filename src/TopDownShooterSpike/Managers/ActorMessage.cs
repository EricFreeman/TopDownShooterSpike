using System;
using TopDownShooterSpike.Simulation;

namespace TopDownShooterSpike.Managers
{
    public abstract class ActorMessage
    {
        private Actor _source;

        public Actor Source
        {
            get { return _source; }
            set
            {
                if(_source != null)
                    throw new InvalidOperationException();

                _source = value;
            }
        }
    }
}