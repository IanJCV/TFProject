using UnityEngine;

namespace TF.Audio
{
    public class Sound
    {
        private static Sound _instance;
        
        private static float _globalVolume;
        
        private static float _sfxVolume;
        private static float _voiceVolume;
        private static float _combatVolume;
        private static float _musicVolume;
        
        private static SoundDevice _device = new NullSoundDevice();

        public static Sound CreateInstance()
        {
            if (_instance is null)
                return new Sound();
            return null;
        }
        
        private Sound()
        {
            _device = new TeleportingSoundDevice();
            _instance = this;
        }
        
        public static void EmitSound(Vector3 position, float volume, AudioClip clip, SoundType type = SoundType.BASIC)
        {
            switch (type)
            {
                case SoundType.BASIC:
                    volume *= _sfxVolume;
                    break;
                case SoundType.VOICE:
                    volume *= _voiceVolume;
                    break;
                case SoundType.COMBAT:
                    volume *= _combatVolume;
                    break;
                case SoundType.MUSIC:
                    volume *= _musicVolume;
                    break;
            }

            volume *= _globalVolume;
            
            _device.Play(position, volume, clip);
        }
    }

    public enum SoundType
    {
        BASIC,
        VOICE,
        COMBAT,
        MUSIC
    }
}