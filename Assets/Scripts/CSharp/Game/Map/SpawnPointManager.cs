using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Mirror;
using kobk.csharp.network;
using kobk.csharp.game.player;

namespace kobk.csharp.game.map
{
    public class SpawnPointManager : NetworkBehaviour
    {
        [SerializeField] private GameObject playerPrefab = null;

        private NetworkManagerLobby _room;
        private NetworkManagerLobby room
        {
            get
            {
                if (_room != null) return _room;
                return _room = NetworkManager.singleton as NetworkManagerLobby;
            }
        }

        private static List<SpawnPoint> spawnpoints = new List<SpawnPoint>();

        private static int curIndex = 0;

        public static void addSpawnPoint(SpawnPoint p) => spawnpoints.Add(p);
        public static void removeSpawnPoint(SpawnPoint p) => spawnpoints.Remove(p);

        public override void OnStartServer() => NetworkManagerLobby.onServerReady += spawnPlayer;

        [ServerCallback]
        public void OnDestroy() => NetworkManagerLobby.onServerReady -= spawnPlayer;

        [Server]
        public void spawnPlayer(NetworkConnection conn)
        {
            SpawnPoint point = spawnpoints.ElementAtOrDefault(curIndex);

            if(point == null) return;

            GameObject playInstance = Instantiate(playerPrefab, spawnpoints[curIndex].transform.position, spawnpoints[curIndex].transform.rotation);
            room.gamePlayerListings.TryToGetValue(conn.identity.GetComponent<GamePlayerBase>(), out int id);
            playInstance.GetComponent<GamePlayerCharacter>().id = id;

            NetworkServer.Spawn(playInstance, conn);

            room.gameCharacterListings.TryAdd(id, playInstance.GetComponent<GamePlayerCharacter>());

            curIndex++;
        }

    }
}