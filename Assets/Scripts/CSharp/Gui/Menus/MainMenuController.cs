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
        public static string[] GameModeOptions = {"Normal", "All Ninjas", "All Ninja Teams"};

        //Serialized Variables
        [Header("Related Objects")]
        [SerializeField] private NetworkManagerLobby networkManager = null;
        [SerializeField] private TelepathyTransport teleTransport;

        [Header("Play settings")]
        [SerializeField] private int minPlayers = 2;

        [Header("Menus")]
        [SerializeField] private GameObject[] Menus;
        [SerializeField] private int start_menu = 0;

        [Header("Error Menu")]
        [SerializeField] private TextMeshProUGUI errorTextDump = null;

        [Header("Connect Menu")]
        [SerializeField] private TMP_InputField IpIn;
        [SerializeField] private TMP_InputField Port_Con;
        [SerializeField] private TMP_InputField Name_con;

        [Header("Host Menu")]
        [SerializeField] private TMP_InputField Port_in;
        [SerializeField] private IntSelector MaxPlayer;
        [SerializeField] private TMP_InputField Name_host;
        [SerializeField] private TextMeshProUGUI IpDump_Host;

        [Header("Connecting Load")]
        [SerializeField] private TextMeshProUGUI connectingLoadIpDump;

        [Header("Lobby Menu")]
        [SerializeField] private Button startBtn;
        [SerializeField] private TextChangingButton startTCBtn;
        [SerializeField] private TextChangingButton readBtn;
        [SerializeField] private TMP_InputField NameChangeField;
        [SerializeField] private GameObject HostGameModeFolder;
        [SerializeField] private GameObject ClientGameModeFolder;
        [SerializeField] private TMP_Dropdown GameModeChangeField;
        [SerializeField] private TextMeshProUGUI GameModeClientField;
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

        // Start is called before the first frame update
        void Start()
        {

            for (int x = 0; x < Menus.Length; x++)
            {
                Menus[x].SetActive(false);
            }

            if (active != null)
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
            // if (n == 4)
            // {
            //     connectingLoadIpDump.text = connectedIP;
            // } 
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

            if (string.IsNullOrEmpty(ip) || string.IsNullOrEmpty(name) || !(ushort.TryParse(Port_in.text, out port)))
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

        public void setupSettingMenu() {
            if(isHosting) {
                ClientGameModeFolder.SetActive(false);
                GameModeChangeField.AddOptions(GameModeOptions.ToList());
                GameModeChangeField.value = 0;
                GameModeChangeField.RefreshShownValue();
            } else {

                //Gamemode change
                HostGameModeFolder.SetActive(false);
            }
        }

        public void updateSettingMenu() {
            LobbyListItem i = networkManager.getListingWithAuthority();

            if(isHosting) {
                GameModeChangeField.value = (int) i.mode;
                GameModeChangeField.RefreshShownValue();
            } else {
                GameModeClientField.text = GameModeOptions[(int) i.mode];
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
                    startTCBtn.selectedText = isLeader ? 2 : 1;
                }
            }
            else
            {
                if (prepedForReady)
                {
                    prepedForReady = false;

                    startBtn.interactable = false;
                    startTCBtn.selectedText = 0;
                }
            }
        }

        public bool isEveryoneReady()
        {
            foreach (var player in networkManager.playerListings)
            {
                if (!player.isReady) return false;
            }
            return true;
        }

        public void Ready()
        {
            LobbyListItem i = networkManager.getListingWithAuthority();
            i.CmdSetReady(!i.isReady);
        }

        public void tryChangeName()
        {
            networkManager.getListingWithAuthority().CmdSetDisplayName(NameChangeField.text);
        }

        public void tryChangeGameMode()
        {
            networkManager.getListingWithAuthority().CmdChangeMode((GameModes)GameModeChangeField.value);
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
            networkManager.getListingWithAuthority().CmdStartGame();
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