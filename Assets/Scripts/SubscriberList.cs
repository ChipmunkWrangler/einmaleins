using System;
using System.Collections.Generic;
using UnityEngine;

internal class SubscriberList<T>
{
    private readonly List<T> subscribers = new List<T>();

    public SubscriberList(GameObject[] subscriberContainers)
    {
        foreach (var subscriberContainer in subscriberContainers)
        foreach (var subscriber in subscriberContainer.GetComponents<T>())
            subscribers.Add(subscriber);
    }

    public void Notify(Action<T> handler)
    {
        foreach (var subscriber in subscribers) handler(subscriber);
    }
}