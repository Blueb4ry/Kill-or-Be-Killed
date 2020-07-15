using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;
using kobk.csharp.gui.fields;
using kobk.csharp.network;
using kobk.csharp.game.enumerations;

namespace kobk.csharp.gui.controller
{

    public class MainMenuController : MonoBehaviour
    {
        //static variables
        public static MainMenuController active = null;
        public static string[] GameModeOptions = { "Normal", "All Ninjas", "All Ninja Teams" };

        //Serialized Variables
        [Header("Related Objects")]
        [SerializeField] private NetworkManagerLobby networkManager = null;
        [SerializeField] private TelepathyTransport teleTransport = null;

        [Header("Play settings")]
        [SerializeField] private int minPlayers = 2;

        [Header("Menus")]
        [SerializeField] private GameObject[] Menus = null;
        [SerializeField] private int start_menu = 0;

        [Header("Error Menu")]
        [SerializeField] private TextMeshProUGUI errorTextDump = null;

        [Header("Connect Menu")]
        [SerializeField] private TMP_InputField IpIn = null;
        [SerializeField] private TMP_InputField Port_Con = null;
        [SerializeField] private TMP_InputField Name_con = null;

        [Header("Host Menu")]
        [SerializeField] private TMP_InputField Port_in = null;
        [SerializeField] private IntSelector MaxPlayer = null;
        [SerializeField] private TMP_InputField Name_host = null;
        [SerializeField] private TextMeshProUGUI IpDump_Host = null;

        [Header("Connecting Load")]
        [SerializeField] private TextMeshProUGUI connectingLoadIpDump = null;

        [Header("Lobby Menu")]
        [SerializeField] private GameObject ParentForLobbyListings = null;
        [SerializeField] private Button startBtn = null;
        [SerializeField] private TextChangingButton startTCBtn = null;
        [SerializeField] private ImageChangingScript startIMCBtn = null;
        [SerializeField] private TextChangingButton readBtn = null;
        [SerializeField] private TMP_InputField NameChangeField = null;
        [SerializeField] private GameObject HostGameModeFolder = null;
        [SerializeField] private GameObject ClientGameModeFolder = null;
        [SerializeField] private TMP_Dropdown GameModeChangeField = null;
        [SerializeField] private TextMeshProUGUI GameModeClientField = null;
        //[SerializeField] public Transform refTrans;

        //hidden variables
        //////////////////

        //Menus
        private int sel;    //the current selected menu
        //Player settings
        public string playerName { get; private set; } = string.Empty;
        //Client Settings
        private string connectedIP = string.Empty;
        //Host settings
        private ushort port = 7777;
        private int MaxPlayers = -1;
        private bool isLeader = false;
        private bool isHosting = false;
        [HideInInspector]
        public bool prepedForReady = false;
        private string hostIPCheck = string.Empty;
        public int NetId = 0;

        // Start is called before the first frame update
        void Start()
        {

            for (int x = 0; x < Menus.Length; x++)
            {
                Menus[x].SetActive(false);
            }

            if (active != null)
            {
                networkManager = NetworkManager.singleton as NetworkManagerLobby;
                teleTransport = networkManager.gameObject.GetComponent<TelepathyTransport>();
                networkManager.ParentFolder = ParentForLobbyListings.transform;
                hostIPCheck = active.hostIPCheck;

                if (networkManager.isNetworkActive)
                {
                    sel = 5;
                    moveTo(5);

                    playerName = active.playerName;
                    connectedIP = active.connectedIP;
                    port = active.port;
                    MaxPlayers = active.MaxPlayers;
                    isLeader = active.isLeader;
                    isHosting = false;
                    prepedForReady = false;
                    NetId = active.NetId;

                } else {
                    throwErrorScreen( string.IsNullOrEmpty(NetworkManagerLobby.QuitReason)? "Unexpected Disconnect" : NetworkManagerLobby.QuitReason); 
                }

                active = this;
            }
            else
            {
                sel = start_menu;
                Menus[sel].SetActive(true);
                active = this;
            }
        }

        void OnEnable()
        {
            NetworkManagerLobby.OnClientConnected += ClientConnectCallback;
            NetworkManagerLobby.onClientDisconnected += ClientDisconnectCallback;
        }

        void OnDisable()
        {
            NetworkManagerLobby.OnClientConnected -= ClientConnectCallback;
            NetworkManagerLobby.onClientDisconnected -= ClientDisconnectCallback;
            active = null;
        }

        //Changes the selected menu and displays it
        public void moveTo(int n)
        {
            Menus[sel].SetActive(false);
            sel = n;
            Menus[sel].SetActive(true);

            switch (n)
            {
                case 2:
                    if (string.IsNullOrEmpty(hostIPCheck))
                    {
                        hostIPCheck = new WebClient().DownloadString("http://icanhazip.com").Trim();
                    }
                    IpDump_Host.text = hostIPCheck;
                    break;
                case 4:
                    connectingLoadIpDump.text = connectedIP;
                    break;
                case 5:
                    setupSettingMenu();
                    updateSettingMenu();
                    break;
            }
        }

        //Quits the game
        public void quitApp()
        {
            Application.Quit();
        }

        void throwErrorScreen(string msg)
        {
            moveTo(3);
            if (networkManager.isNetworkActive)
                forceDisconnect();
            errorTextDump.text = msg;
        }

        //Joins the gmae with the selected ip and name
        public void joinGameSelected()
        {
            //TODO: load lobby

            //Checks for valid string ip
            string ip = IpIn.text.Trim();
            string name = Name_con.text.Trim();

            if (string.IsNullOrEmpty(ip) || string.IsNullOrEmpty(name) || !(ushort.TryParse(Port_Con.text, out port)))
            {
                //if bad move to error
                throwErrorScreen("You must enter an ip, name and port");
                return;
            }

            playerName = name;
            connectedIP = ip;
            teleTransport.port = port;

            moveTo(4);
            isLeader = false;

            networkManager.networkAddress = ip;
            networkManager.StartClient();
            connectedIP = ip + ":" + port;
        }

        //Starts a game as the host
        public void startGameHost()
        {
            //TODO: load lobby

            //check for valid information
            bool invalid = false;
            string reason = "Cannot start host because: ";

            if (!(MaxPlayer.value >= minPlayers))
            {
                invalid = true;
                reason += "invalid max player value, ";
            }
            else
            {
                MaxPlayers = MaxPlayer.value;
                networkManager.maxConnections = MaxPlayers;
            }

            if (!ushort.TryParse(Port_in.text, out port))
            {
                invalid = true;
                reason += "invalid port value, ";
            }
            else
            {
                teleTransport.port = port;
            }

            string name = Name_host.text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                invalid = true;
                reason += "must enter name, ";
            }
            else
            {
                playerName = name;
            }

            if (invalid)
            {
                throwErrorScreen(reason);
                return;
            }

            connectedIP = "localhost:" + port.ToString();
            isLeader = true;
            isHosting = true;

            moveTo(4);
            networkManager.StartHost();

        }

        public void setupSettingMenu()
        {
            if (isHosting)
            {
                ClientGameModeFolder.SetActive(false);
                GameModeChangeField.AddOptions(GameModeOptions.ToList());
                GameModeChangeField.value = 0;
                GameModeChangeField.RefreshShownValue();
            }
            else
            {
                //Gamemode change
                HostGameModeFolder.SetActive(false);
            }
        }

        public void updateSettingMenu()
        {
            //LobbyListItem i = networkManager.getListingWithAuthority();
            networkManager.playerListings.TryToGetValue(NetId, out LobbyListItem i);
            if(i == null) return;

            if (isHosting)
            {
                GameModeChangeField.value = (int)i.mode;
                GameModeChangeField.RefreshShownValue();
            }
            else
            {
                GameModeClientField.text = GameModeOptions[(int)i.mode];
            }
        }

        public void updateReadyButton(bool s)
        {
            readBtn.selectedText = s ? 1 : 0;
        }

        public void ReadyCheck()
        {
            //check if everyone is ready

            if (networkManager.hasEnoughPlayers() && isEveryoneReady())
            {
                if (!prepedForReady)
                {
                    prepedForReady = true;

                    startBtn.interactable = isLeader;
                    startIMCBtn.selectedImg = isLeader ? 1 : 0;
                    startTCBtn.selectedText = isLeader ? 2 : 1;
                }
            }
            else
            {
                if (prepedForReady)
                {
                    prepedForReady = false;

                    startBtn.interactable = false;
                    startIMCBtn.selectedImg = 0;
                    startTCBtn.selectedText = 0;
                }
            }
        }

        public bool isEveryoneReady()
        {
            foreach (var player in networkManager.playerListings.Values)
            {
                if (!player.isReady) return false;
            }
            return true;
        }

        public void Ready()
        {
            //LobbyListItem i = networkManager.getListingWithAuthority();
            networkManager.playerListings.TryToGetValue(NetId, out LobbyListItem i);
            i.CmdSetReady(!i.isReady);
        }

        public void tryChangeName()
        {
            /*networkManager.getListingWithAuthority()*/
            networkManager.playerListings.TryToGetValue(NetId, out LobbyListItem i);
            i.CmdSetDisplayName(NameChangeField.text);
        }

        public void tryChangeGameMode()
        {
            /*networkManager.getListingWithAuthority()*/
            networkManager.playerListings.TryToGetValue(NetId, out LobbyListItem i);
            i.CmdChangeMode((GameModes)GameModeChangeField.value);
        }

        public void DisconnectMenu()
        {
            forceDisconnect();
            moveTo(0);
        }

        public void forceDisconnect()
        {
            if (isHosting)
            {
                networkManager.StopHost();
            }
            else
            {
                networkManager.StopClient();
            }
        }

        public void attemptStart()
        {
            /*networkManager.getListingWithAuthority()*/
            networkManager.playerListings.TryToGetValue(NetId, out LobbyListItem i);
            i.CmdStartGame();
        }

        private void ClientConnectCallback()
        {
            //throwErrorScreen("Success!");
            Debug.Log("Connection successful");

            moveTo(5);
        }

        private void ClientDisconnectCallback()
        {
            throwErrorScreen("Unexepcted disconnect");
        }

    }
}