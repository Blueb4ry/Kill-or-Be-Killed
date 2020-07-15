using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using kobk.csharp.gui.controller;
using kobk.csharp.game.player;
using kobk.csharp.collections;

namespace kobk.csharp.network
{
    public class NetworkManagerLobby : NetworkManager
    {
        //static constant variables
        public const string LoadFolder = "Spawnables";
        public static string QuitReason = string.Empty;

        //Serialized Variables
        [SerializeField] private int minPlayers = 2;
        [SerializeField] private string menuScene = string.Empty;

        [Header("Room")]
        [SerializeField] public Transform ParentFolder = null;
        [SerializeField] private LobbyListItem LobbyPlayerPrefab = null;

        [Header("Game")]
        [SerializeField] private string[] gameScene = null;
        [SerializeField] private string gameScenePrefix = null;
        [SerializeField] public int selectedGameScene = 0;
        [SerializeField] private GamePlayerBase gamePlayerPrefab = null;
        [SerializeField] private GameObject SpawnSystemPrefab = null;

        //hidden variables

        //functionality
        public DualDictionary<int, LobbyListItem> playerListings { get; } = new DualDictionary<int, LobbyListItem>();
        public DualDictionary<int, GamePlayerBase> gamePlayerListings { get; } = new DualDictionary<int, GamePlayerBase>();
        public DualDictionary<int, GamePlayerCharacter> gameCharacterListings { get; } = new DualDictionary<int, GamePlayerCharacter>();
        private bool accepting = true;

        //events
        public static event Action OnClientConnected;
        public static event Action onClientDisconnected;

        public static event Action<NetworkConnection> onServerReady;

        //methods

        public override void Awake()
        {

            if (NetworkManager.singleton != null)
            {
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

            // if(SceneManager.GetActiveScene().name != menuScene) {
            //     SceneManager.LoadSceneAsync(menuScene, LoadSceneMode.Single);
            // }
        }

        public override void OnServerConnect(NetworkConnection conn)
        {
            if (numPlayers >= maxConnections || SceneManager.GetActiveScene().name != menuScene || !accepting)
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

                int newid = 0;
                do{
                    newid = new System.Random().Next();
                } while(playerListings.containsKey(newid));

                PlayerInstance.id = newid;
                

                NetworkServer.AddPlayerForConnection(conn, PlayerInstance.gameObject);
            }
        }

        public override void OnServerDisconnect(NetworkConnection conn)
        {
            if (conn.identity != null)
            {
                if (SceneManager.GetActiveScene().name.StartsWith(gameScenePrefix))
                {
                    var player = conn.identity.GetComponent<GamePlayerBase>();

                    gamePlayerListings.Remove(player);
                    //gamePlayerListings.Remove(gamePlayerListings.)
                }
                else
                {
                    var player = conn.identity.GetComponent<LobbyListItem>();

                    playerListings.Remove(player);
                }
            }

            base.OnServerDisconnect(conn);
        }

        public override void OnStopServer()
        {
            playerListings.Clear();
            gamePlayerListings.Clear();
        }

        [Obsolete("This method is obsolete, use player id to grab from DualDictionary", false)]
        public LobbyListItem getListingWithAuthority()
        { 
            foreach (var player in playerListings.Values)
            {
                if (player.hasAuthority)
                    return player;
            }
            return null;
        }

        [Obsolete("This method is obsolete, use player id to grab from DualDictionary", false)]
        public GamePlayerBase getGameListingWithAuthority() {
            foreach (var player in gamePlayerListings.Values)
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
            if (!MainMenuController.active.isEveryoneReady()) return;

            if (SceneManager.GetActiveScene().name == menuScene)
            {

                accepting = false;
                ServerChangeScene(gameScene[selectedGameScene]);
            }

        }

        public override void ServerChangeScene(string newScene)
        {

            if (SceneManager.GetActiveScene().name == menuScene && newScene.StartsWith(gameScenePrefix))
            {
                int[] ids = playerListings.Keys.ToArray();
                //for (int x = playerListings.Count - 1; x >= 0; x--)
                for (int x = 0; x < ids.Length; x++)
                {
                    //var roomPlayer = playerListings[x];
                    playerListings.TryToGetValue(ids[x], out var roomPlayer);
                    var conn = roomPlayer.connectionToClient;
                    var gamePlayerInstance = Instantiate(gamePlayerPrefab);
                    gamePlayerInstance.setupData(roomPlayer.username, roomPlayer.mode, roomPlayer.team, roomPlayer.id);

                    NetworkServer.Destroy(conn.identity.gameObject);

                    NetworkServer.ReplacePlayerForConnection(conn, gamePlayerInstance.gameObject);
                }
            }

            base.ServerChangeScene(newScene);
        }

        public override void OnServerSceneChanged(string SceneName)
        {
            if (SceneName.StartsWith(gameScenePrefix))
            {
                GameObject SpawnSystemInstance = Instantiate(SpawnSystemPrefab);
                NetworkServer.Spawn(SpawnSystemInstance);
            }
        }

        public override void OnServerReady(NetworkConnection conn)
        {
            base.OnServerReady(conn);

            onServerReady?.Invoke(conn);
        }

    }
}