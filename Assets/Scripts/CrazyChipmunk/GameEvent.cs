using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace CrazyChipmunk
{
    [CreateAssetMenu(menuName = "CrazyChipmunk/GameEvent")]
    public class GameEvent : ScriptableObject
    {
        List<GameEventSubscriber> subscribers = new List<GameEventSubscriber>();

        public void Raise()
        {
            for (int i = subscribers.Count - 1; i >= 0; --i)
            {
                Debug.Log("Sending " + name + " to " + subscribers[i].name);
                subscribers[i].OnEvent();
            }
        }

        public void Subscribe(GameEventSubscriber subscriber)
        {
            if (subscribers.Contains(subscriber))
            {
                Debug.LogError("Duplicate subscriber to " + name + ": " + subscriber.name);
            }
            else
            {
                Debug.Log("Subscribing " + subscriber.name + " to " + name);
                subscribers.Add(subscriber);
            }
        }

        public void Unsubscribe(GameEventSubscriber subscriber)
        {
            Debug.Log("Unsubscribing " + subscriber.name + " from " + name);
            subscribers.Remove(subscriber);
        }
    }
}