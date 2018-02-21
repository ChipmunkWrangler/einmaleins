using System;
using System.Collections.Generic;
using UnityEngine;

public class SubscriberList<T>
{
    readonly List<T> Subscribers = new List<T>();

    public SubscriberList(GameObject[] subscriberContainers)
    {
        foreach (GameObject subscriberContainer in subscriberContainers)
        {
            foreach (T subscriber in subscriberContainer.GetComponents<T>())
            {
                Subscribers.Add(subscriber);
            }
        }
    }

    public void Notify(Action<T> handler)
    {
        foreach (T subscriber in Subscribers)
        {
            handler(subscriber);
        }
    }
}