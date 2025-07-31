using UnityEngine;
using UnityEngine.Audio;

namespace AudioSystem
{
    [System.Serializable]
    public class SoundData
    {
        public AudioClip clip;
        public AudioMixerGroup group;
        public bool loop;
        public bool playOnAwake;
        public bool frequentSound;
    }

}

