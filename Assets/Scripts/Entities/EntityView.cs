using UnityEngine;
using TF.Audio;

namespace TF.Entities
{
    public abstract class EntityView : MonoBehaviour
    {
        private Transform _transform;
        private EntityBase _entity;

        public EntityBase GetBase() => _entity;

        public Vector3 GetPosition() => _transform.position;
        public Vector3 GetForward() => _transform.forward;
        public Vector3 GetRight() => _transform.right;
        
        public void EmitSound(AudioClip clip, float volume, SoundType type) 
            => Sound.EmitSound(_transform.position, volume, clip, type);

        public virtual void Precache()
        {
            _transform = transform;
        }
    }
}