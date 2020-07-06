using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Mirror;
using kobk.csharp.network;

namespace kobk.csharp.game.map
{
    public class SpawnPointManager : NetworkBehaviour
    {
        [SerializeField] private GameObject playerPrefab;

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
            NetworkServer.Spawn(playInstance, conn);

            curIndex++;
        }

    }
}