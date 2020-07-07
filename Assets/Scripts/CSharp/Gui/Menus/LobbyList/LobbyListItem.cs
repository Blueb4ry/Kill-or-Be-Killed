using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;
using kobk.csharp.game.enumerations;
using kobk.csharp.network;

namespace kobk.csharp.gui.controller
{
    public class LobbyListItem : NetworkBehaviour
    {
        //Index meanings see Gamemode
        public static string[][] ModeTeamOptions = { new string[] { "Random", "Soliders", "Ninjas" }, new string[] { "Random", "Team A", "Team B" } };

        //serialized variables
        [SerializeField] private TextMeshProUGUI NameField;
        [SerializeField] private TextMeshProUGUI ReadyInd;
        [SerializeField] private TMP_Dropdown TeamSel;
        [SerializeField] private string readyText;
        [SerializeField] private string notReadyText;
        [SerializeField] private Color readyColor;
        [SerializeField] private Color notReadyColor;

        public bool isLeader = false;

        //properties
        [SyncVar(hook = nameof(HandleReadyChange))]
        public bool isReady = false;
        // public bool isReady
        // {
        //     get { return _isReady; }
        //     set
        //     {
        //         if (_isReady != value)
        //         {
        //             _isReady = value;
        //             if (_isReady)
        //             {
        //                 ReadyInd.color = readyColor;
        //                 ReadyInd.text = readyText;
        //             }
        //             else
        //             {
        //                 ReadyInd.color = notReadyColor;
        //                 ReadyInd.text = notReadyText;
        //             }
        //         }
        //     }
        // }

        [SyncVar(hook = nameof(HandleNameChange))]
        public string username = string.Empty;
        // public string username
        // {
        //     get { return _username; }
        //     set
        //     {
        //         _username = value;
        //         NameField.text = _username;
        //     }
        // }

        [SyncVar(hook = nameof(HandleModeChange))]
        public GameModes mode = GameModes.NORMAL;
        //public GameModes mode
        // {
        //     get { return _mode; }
        //     set
        //     {
        //         if (_mode != value)
        //         {
        //             _mode = value;
        //             TeamSel.ClearOptions();
        //             foreach (string opt in ModeTeamOptions[(int)_mode])
        //             {
        //                 TeamSel.options.Add(new Dropdown.OptionData(opt));
        //             }
        //         }
        //     }
        // }
        [SyncVar(hook = nameof(HandleTeamChange))]
        public int team = 0;

        private NetworkManagerLobby _room;
        private NetworkManagerLobby room
        {
            get
            {
                if (_room != null) return _room;
                return _room = NetworkManager.singleton as NetworkManagerLobby;
            }
        }

        public override void OnStartAuthority()
        {
            CmdSetDisplayName(MainMenuController.active.playerName);
        }

        public override void OnStartClient()
        {
            room.playerListings.Add(this);
            //this.gameObject.transform.parent = room.ParentFolder;
            this.gameObject.transform.SetParent(room.ParentFolder);
            this.gameObject.transform.localScale = Vector3.one;

            UpdateUI();
        }

        public override void OnStopClient()
        {
            room.playerListings.Remove(this);

            UpdateUI();
        }

        public void HandleNameChange(string old, string newV) => UpdateUI();
        public void HandleReadyChange(bool old, bool newV)
        {
            UpdateUI();

            if(hasAuthority)
                MainMenuController.active.updateReadyButton(isReady);
            MainMenuController.active.ReadyCheck();
        }
        public void HandleModeChange(GameModes old, GameModes newV) {
            UpdateUI();
            MainMenuController.active.updateSettingMenu();
        }
        public void HandleTeamChange(int old, int newV) => UpdateUI();

        public void UpdateUI()
        {
            //update name
            NameField.text = username;

            //update ready state
            if (isReady)
            {
                ReadyInd.color = readyColor;
                ReadyInd.text = readyText;
            }
            else
            {
                ReadyInd.color = notReadyColor;
                ReadyInd.text = notReadyText;
            }

            //update mode dropdowns
            TeamSel.ClearOptions();
            //List<string> newOpts = new List<string>();
            // foreach (string opt in ModeTeamOptions[(int)mode])
            // {
            //     newOpts.Add(new Dropdown.OptionData(opt));
            // }
            TeamSel.AddOptions(ModeTeamOptions[(int)mode].ToList());
            TeamSel.value = team;
            TeamSel.RefreshShownValue();

            MainMenuController.active.updateSettingMenu();
        }

        public void onDropdownValueChange() {
            CmdTeamChange(TeamSel.value);
        }

        //commands
        [Command]
        public void CmdSetDisplayName(string displayname)
        {
            username = displayname;
        }

        [Command]
        public void CmdSetReady(bool s)
        {
            isReady = s;
        }

        [Command]
        public void CmdTeamChange(int t) {
            team = t;
        }

        [Command]
        public void CmdChangeMode(GameModes g) {
            if(!isLeader) return;

            foreach(var player in room.playerListings) {
                player.mode = g;
                player.team = 0;
            }
        }

        [Command]
        public void CmdStartGame()
        {
            if(!isLeader) return;

            //start Game
            Debug.Log("Starting Game...");
            room.StartGame();
        }

    }
}