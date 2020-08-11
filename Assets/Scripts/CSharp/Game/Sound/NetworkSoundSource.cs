using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kobk.csharp.game.sound
{
    [RequireComponent(typeof(AudioSource))]
    public class NetworkSoundSource : MonoBehaviour
    {
        public AudioSource source = null;
        public AudioClip[] clips = null;

        public NetworkSoundSystem connectedSystem = null;
        public int SourceId = -1;

        public void RequestNetworkPlay(int clipId, bool loop = true) {
            connectedSystem.CmdRequestPlayback(SourceId, clipId, loop);
        }

        public void PlayImmediately(int trackId, bool loop = true) {
            source.loop = loop;
            source.clip = clips[trackId];
            source.Play();
        }

        public void FroceStop() => source.Stop();

    }
}