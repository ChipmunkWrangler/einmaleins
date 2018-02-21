using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnAswerChangedNotifier
{
    List<OnAnswerChanged> onAnswerChangedSubscribers = new List<OnAnswerChanged>();

    public void Subscribe(OnAnswerChanged newSubscriber)
    {
        onAnswerChangedSubscribers.Add(newSubscriber);
    }

    public void NotifySubscribers(bool isTextEmpty)
    {
        foreach (OnAnswerChanged subscriber in onAnswerChangedSubscribers)
        {
            subscriber.OnAnswerChanged(isTextEmpty);
        }
    }
}
