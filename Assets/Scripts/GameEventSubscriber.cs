using UnityEngine;
using UnityEngine.Events;

namespace CrazyChipmunk
{
    public class GameEventSubscriber : MonoBehaviour
    {
        [SerializeField] GameEvent subscribeTo = null;
        [SerializeField] UnityEvent respondWith = null;

    public void OnGameEvent() {
            respondWith.Invoke();
        }

        void OnEnable()
        {
            subscribeTo.Subscribe(this);
        }

        private void OnDisable()
        {
            subscribeTo.Unsubscribe(this);
        }
    }
}