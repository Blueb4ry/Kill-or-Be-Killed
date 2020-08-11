using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace kobk.csharp.game.sound
{
    public class NetworkSoundSystem : NetworkBehaviour
    {
        public static NetworkSoundSystem singleton = null;
        public List<NetworkSoundSource> sources = new List<NetworkSoundSource>();

        private void Awake() {
            if(singleton is null) {
                singleton = this;
            } else {
                Debug.LogError("There is multiple NetworkSoundSystem objects, please ensure there is only one");
            }
        }

        private void OnDestroy()
		{
            if(singleton == this)
                singleton = null; 
		}

		[Command]
        public void CmdRequestPlayback(int SourceId, int trackid, bool loop) => RpcPlayback(SourceId, trackid, loop);
        

        [ClientRpc]
        public void RpcPlayback(int SourceId, int trackid, bool loop) {
            sources[SourceId].PlayImmediately(trackid, loop);
        }

        [Command]
        public void CmdRequestStop(int SourceId) => RpcStop(SourceId);

        [ClientRpc]
        public void RpcStop(int SourceId) {
            sources[SourceId].FroceStop();
        }

    }
}