using FarseerPhysics.Dynamics;
using TopDownShooterSpike.Graphics;
using TopDownShooterSpike.Simulation;

namespace TopDownShooterSpike.Managers
{
    public interface IActorService
    {
        IActorEventAggregator EventAggregator { get; }
        IDeviceInputService InputService { get; }
        IRenderManager RenderManager { get; }
        World PhysicsSystem { get; }
        SimulationSettings SimulationSettings { get; }

        T CreateRenderObject<T>() where T : RenderObject;
    }
}