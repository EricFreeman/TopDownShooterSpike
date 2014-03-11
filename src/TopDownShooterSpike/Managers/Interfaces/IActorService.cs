using FarseerPhysics.Dynamics;
using TopDownShooterSpike.Graphics;
using TopDownShooterSpike.Simulation;

namespace TopDownShooterSpike.Managers
{
    public interface IActorService
    {
        IActorEventAggregator EventAggregator { get; }
        IDeviceInputService InputService { get; }
        SimulationSettings SimulationSettings { get; }
        World PhysicsSystem { get; }
        T CreateRenderObject<T>() where T : RenderObject;
    }
}