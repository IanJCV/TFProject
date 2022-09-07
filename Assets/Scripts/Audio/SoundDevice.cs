using UnityEngine;

namespace TF.Audio
{
    public abstract class SoundDevice
    {
        public abstract void Play(Vector3 pos, float vol, AudioClip clip);
    }
    
    public class NullSoundDevice : SoundDevice
    {
        public override void Play(Vector3 pos, float vol, AudioClip clip)
        {
            return;
        }
    }

    /// <summary>
    /// Pooled device logic.
    /// <para>Should create a number of managed sound emitters
    /// that will teleport to the correct position, play sounds,
    /// and get put back into the pool when they're done.</para>
    /// </summary>
    public class PooledSoundDevice : SoundDevice
    {
        public override void Play(Vector3 pos, float vol, AudioClip clip)
        {
        }
    }

    /// <summary>
    /// Basic global sound.
    /// </summary>
    public class TeleportingSoundDevice : SoundDevice
    {
        
        public override void Play(Vector3 pos, float vol, AudioClip clip)
        {
            AudioSource.PlayClipAtPoint(clip, pos, vol);
        }
    }
}