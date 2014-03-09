using System;
using System.Collections.Generic;
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

    public class ActorEventAggregator : IActorEventAggregator
    {
        private readonly Dictionary<Type, IList<object>> _listenerMap = new Dictionary<Type, IList<object>>();

        public void SendMessage<T>(Actor source, T message = null) where T : ActorMessage, new()
        {
            var type = typeof (T);
            var listenersForType = _listenerMap[type];

            if(message == null)
                message = new T();

            foreach (var listener in listenersForType)
            {
                var a = listener as IListener<T>;

                if (a != null) 
                    a.Handle(message);
            }
        }

        public void Listen<T>(IListener<T> listener) where T : ActorMessage
        {
            var type = typeof (T);
            IList<object> listeners;

            if (_listenerMap.ContainsKey(type))
                listeners = _listenerMap[type];
            else
                _listenerMap.Add(type, listeners = new List<object>(8));

            listeners.Add(listener);
        }
    }
}