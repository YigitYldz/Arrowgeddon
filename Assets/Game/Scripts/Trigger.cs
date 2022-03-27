using UnityEngine;
using UnityEngine.Events;

namespace SSZone
{
    public class Trigger : MonoBehaviour
    {
        public TriggerEvent onTriggerEnter = new TriggerEvent();

        private void OnTriggerEnter(Collider other)
        {
            onTriggerEnter?.Invoke(other);
        }
    }

    public class TriggerEvent : UnityEvent<Collider> { }
}
