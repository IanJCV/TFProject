using System;
using UnityEngine;
using UnityEngine.Events;

namespace TF.Physics
{
    public class TriggerBox : MonoBehaviour
    {
        public UnityEvent<GameObject> onEnter = new UnityEvent<GameObject>();
        public UnityEvent<GameObject> onStay = new UnityEvent<GameObject>();
        public UnityEvent<GameObject> onExit = new UnityEvent<GameObject>();

        private void OnTriggerEnter(Collider other)
        {
            onEnter.Invoke(other.gameObject);
        }

        private void OnTriggerStay(Collider other)
        {
            onStay.Invoke(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            onExit.Invoke(other.gameObject);
        }
    }
}