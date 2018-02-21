﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class SubscriberList<T>
{
    List<T> subscribers = new List<T>();

    public SubscriberList(GameObject[] subscriberContainers)
    {
        foreach (GameObject subscriberContainer in subscriberContainers)
        {
            foreach (T subscriber in subscriberContainer.GetComponents<T>())
            {
                subscribers.Add(subscriber);
            }
        }
    }

    public void Notify(Action<T> handler)
    {
        foreach (T subscriber in subscribers)
        {
            handler(subscriber);
        }
    }
}