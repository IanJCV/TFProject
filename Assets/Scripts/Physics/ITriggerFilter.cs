using UnityEngine;

namespace TF.Physics
{
    public interface ITriggerFilter
    {
        public bool Filter(GameObject go);
    }
}