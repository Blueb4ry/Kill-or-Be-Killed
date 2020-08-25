using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Mirror;
using kobk.csharp.network;
using kobk.csharp.game.player;
using kobk.csharp.game.enumerations;
using kobk.csharp.gui.game;

namespace kobk.csharp.game.map
{
    public class SpawnPointManager : NetworkBehaviour
    {
        [SerializeField] private GameObject playerPrefab = null;
        [SerializeField] private GameObject NinjaPrefab = null;
        [SerializeField] private GameObject SoldierPrefab = null;

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

            GamePlayerBase baseObject = conn.identity.GetComponent<GamePlayerBase>();
            GameModes mode = baseObject.mode;
            GameObject playInstance = null;
            if (mode == GameModes.NORMAL)
            {
                int team = baseObject.team;
                //if soldier team
                if (team == 1)
                {
                    playInstance = Instantiate(SoldierPrefab, spawnpoints[curIndex].transform.position, spawnpoints[curIndex].transform.rotation);
                    UserUIManager.instance.setUI(UserUIManager.SOLDIER);
                }
                else if (team == 2)
                {
                    playInstance = Instantiate(NinjaPrefab, spawnpoints[curIndex].transform.position, spawnpoints[curIndex].transform.rotation);
                    UserUIManager.instance.setUI(UserUIManager.NINJA);
                }
                else
                {
                    playInstance = Instantiate(NinjaPrefab, spawnpoints[curIndex].transform.position, spawnpoints[curIndex].transform.rotation);
                    UserUIManager.instance.setUI(UserUIManager.NINJA);
                }
            }
            else if (mode == GameModes.ALL_NINJA || mode == GameModes.ALL_NINJA_TEAMS)
            {
                playInstance = playInstance = Instantiate(NinjaPrefab, spawnpoints[curIndex].transform.position, spawnpoints[curIndex].transform.rotation);
                UserUIManager.instance.setUI(UserUIManager.NINJA);
            }
            else 
            {
                playInstance = playInstance = Instantiate(NinjaPrefab, spawnpoints[curIndex].transform.position, spawnpoints[curIndex].transform.rotation);
                UserUIManager.instance.setUI(UserUIManager.NINJA);
            }

            //GameObject playInstance = Instantiate(playerPrefab, spawnpoints[curIndex].transform.position, spawnpoints[curIndex].transform.rotation);
            room.gamePlayerListings.TryToGetValue(baseObject, out int id);
            playInstance.GetComponent<GamePlayerCharacter>().id = id;

            NetworkServer.Spawn(playInstance, conn);

            room.gameCharacterListings.TryAdd(id, playInstance.GetComponent<GamePlayerCharacter>());

            curIndex++;
        }

    }
}