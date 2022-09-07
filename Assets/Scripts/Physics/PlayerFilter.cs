using TF.Entities;
using UnityEngine;

namespace TF.Physics
{
    [CreateAssetMenu(menuName = "Player Filter", fileName = "Game/Physics/Player Filter")]
    public class PlayerFilter : ScriptableObject, ITriggerFilter
    {
        public bool Filter(GameObject go)
        {
            if (go.GetComponent<EntityView>().GetBase().IsPlayer())
                return true;
            return false;
        }
    }
}