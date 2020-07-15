using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using kobk.csharp.game.enumerations;
using kobk.csharp.network;

namespace kobk.csharp.game.player
{
    public class GamePlayerBase : NetworkBehaviour
    {
        [SyncVar]
        public string username = "NameNotFound";
        [SyncVar]
        public GameModes mode = GameModes.NORMAL;
        [SyncVar]
        public int team = 0;
        [SyncVar]
        public int id = -1;

        private NetworkManagerLobby _room;
        private NetworkManagerLobby room
        {
            get
            {
                if (_room != null) return _room;
                return _room = NetworkManager.singleton as NetworkManagerLobby;
            }
        }

        public override void OnStartClient() {
            DontDestroyOnLoad(gameObject);

            room.gamePlayerListings.Add(id, this);
        }

        public override void OnStopClient() {
            room.gamePlayerListings.Remove(this);
        }

        [Server]
        public void setupData(string name, GameModes mode, int team, int id) {
            this.username = name;
            this.mode = mode;
            this.team = team;
            this.id = id;
        }
        
    }
}