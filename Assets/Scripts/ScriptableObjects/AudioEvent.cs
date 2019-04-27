using UnityEngine;

namespace SvS.Audio {
    public abstract class AudioEvent : ScriptableObject {
        public abstract void Play(AudioSource source);
    }
}

