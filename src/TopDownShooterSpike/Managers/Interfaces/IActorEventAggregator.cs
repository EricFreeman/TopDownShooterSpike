using TopDownShooterSpike.Simulation;

namespace TopDownShooterSpike.Managers
{
    public interface IActorEventAggregator
    {
        void SendMessage<T>(Actor source, T message = null) where T : ActorMessage, new();
        void Listen<T>(IListener<T> listener) where T : ActorMessage;
    }

    public interface IListener<in T> where T : ActorMessage
    {
        void Handle(T message);
    }
}