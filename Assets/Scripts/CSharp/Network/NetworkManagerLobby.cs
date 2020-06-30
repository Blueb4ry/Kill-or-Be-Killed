using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using kobk.csharp.gui.controller;
using kobk.csharp.game.player;

namespace kobk.csharp.network
{
    public class NetworkManagerLobby : NetworkManager
    {
        //static constant variables
        public const string LoadFolder = "LobbyLoad";

        //Serialized Variables
        [SerializeField] private int minPlayers = 2;
        [SerializeField] private string menuScene = string.Empty;

        [Header("Room")]
        [SerializeField] public Transform ParentFolder = null;
        [SerializeField] private LobbyListItem LobbyPlayerPrefab = null;

        [Header("Game")]
        [SerializeField] private string[] gameScene;
        [SerializeField] private string gameScenePrefix;
        [SerializeField] public int selectedGameScene = 0;
        [SerializeField] private GamePlayerBase gamePlayerPrefab;

        //hidden variables

        //functionality
        public List<LobbyListItem> playerListings { get; } = new List<LobbyListItem>();
        public List<GamePlayerBase> gamePlayerListings { get; } = new List<GamePlayerBase>();

        //events
        public static event Action OnClientConnected;
        public static event Action onClientDisconnected;

        //methods

        public override void Awake() {

            if(NetworkManager.singleton != null) {
                Destroy(gameObject);
                return;
            }

            base.Awake();
        }

        //calls when server starts scene
        public override void OnStartServer() => spawnPrefabs = Resources.LoadAll<GameObject>(LoadFolder).ToList();

        //calls when client starts scene
        public override void OnStartClient()
        {
            var spawnablePrefabs = Resources.LoadAll<GameObject>(LoadFolder);

            foreach (var prefab in spawnablePrefabs)
            {
                ClientScene.RegisterPrefab(prefab);
            }
        }

        public override void OnClientConnect(NetworkConnection conn)
        {
            base.OnClientConnect(conn);

            OnClientConnected?.Invoke();
        }

        public override void OnClientDisconnect(NetworkConnection conn)
        {
            base.OnClientDisconnect(conn);

            onClientDisconnected?.Invoke();
        }

        public override void OnServerConnect(NetworkConnection conn)
        {
            if (numPlayers >= maxConnections)
            {
                conn.Disconnect();
                return;
            }
            else if (SceneManager.GetActiveScene().name != menuScene)
            {
                conn.Disconnect();
                return;
            }
        }

        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            if (SceneManager.GetActiveScene().name.Equals(menuScene))
            {
                LobbyListItem PlayerInstance = Instantiate(LobbyPlayerPrefab, ParentFolder);

                PlayerInstance.isLeader = playerListings.Count == 0;

                NetworkServer.AddPlayerForConnection(conn, PlayerInstance.gameObject);
            }
        }

        public override void OnServerDisconnect(NetworkConnection conn)
        {
            if (conn.identity != null)
            {
                var player = conn.identity.GetComponent<LobbyListItem>();

                playerListings.Remove(player);
            }

            base.OnServerDisconnect(conn);
        }

        public override void OnStopServer()
        {
            playerListings.Clear();
        }

        public LobbyListItem getListingWithAuthority()
        {
            foreach (var player in playerListings)
            {
                if (player.hasAuthority)
                    return player;
            }
            return null;
        }

        public bool hasEnoughPlayers()
        {
            return numPlayers >= minPlayers;
        }

        public void StartGame()
        {
            if(!MainMenuController.active.isEveryoneReady()) return;

            if (SceneManager.GetActiveScene().name == menuScene)
            {
                ServerChangeScene(gameScene[selectedGameScene]);
            }

        }

        public override void ServerChangeScene(string newScene)
        {

            if (SceneManager.GetActiveScene().name == menuScene && newScene.StartsWith(gameScenePrefix))
            {
                for (int x = playerListings.Count - 1; x >= 0; x--)
                {
                    var roomPlayer = playerListings[x];
                    var conn = roomPlayer.connectionToClient;
                    var gamePlayerInstance = Instantiate(gamePlayerPrefab);
                    gamePlayerInstance.setupData(roomPlayer.username, roomPlayer.mode, roomPlayer.team);

                    NetworkServer.Destroy(conn.identity.gameObject);

                    NetworkServer.ReplacePlayerForConnection(conn, gamePlayerInstance.gameObject);
                }
            } 

            base.ServerChangeScene(newScene);

        }

    }
}