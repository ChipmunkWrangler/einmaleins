using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace CrazyChipmunk
{
    public class GameEvent : ScriptableObject
    {
        List<GameEventSubscriber> subscribers = new List<GameEventSubscriber>();

        public void Notify()
        {
            for (int i = subscribers.Count; i >= 0; --i)
            {
                subscribers[i].OnEvent();
            }
        }

        public void Subscribe(GameEventSubscriber subscriber)
        {
            Assert.IsFalse(subscribers.Contains(subscriber), "Duplicate subscriber to " + name + ": " + subscriber.name);
            subscribers.Add(subscriber);
        }

        public void Unsubscribe(GameEventSubscriber subscriber)
        {
            subscribers.Remove(subscriber);
        }
    }
}