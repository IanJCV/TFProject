using UnityEngine;
using TF.Audio;

namespace TF.Entities
{
    public abstract class EntityBase
    {
        private bool _isPaused;
        private bool _isPlayer;

        private EntityView _view;

        public EntityBase(EntityView view)
        {
            _view = view;
        }
        
        protected virtual bool SetPaused(bool paused) => _isPaused = paused;
        protected void EmitSound(AudioClip clip, float volume, SoundType type) => _view.EmitSound(clip, volume, type);
        
        public bool GetPaused() => _isPaused;
        public virtual bool IsPlayer() => false;


        public Vector3 GetOrigin() => _view.GetPosition();
        public EntityView GetView() => _view;

        public virtual void PreUpdate()
        {
            
        }
        
        public virtual void FrameUpdate()
        {
            
        }

        public virtual void PostUpdate()
        {
            
        }

        ~EntityBase()
        {
            
        }
        
    }
}

/*
 // Only restrict this to Solid layers for performance. 
 

if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit))
            {
                MeshCollider meshCollider = hit.collider as MeshCollider;
                if (meshCollider != null || meshCollider.sharedMesh != null)
                {
                    mesh = meshCollider.sharedMesh;
                    Renderer renderer = hit.collider.GetComponent<MeshRenderer>();
                    
                    for (int i = 0; i < mesh.subMeshCount; i++)
                    {
                        var submesh = mesh.GetSubMesh(i);
                        if (hit.triangleIndex >= submesh.indexStart / 3 && hit.triangleIndex < submesh.indexCount / 3)
                        {
                            mat = renderer.materials[i];
                            // We have found the material. No need to loop through every triangle.
                        }
                    }
                }
}

*/