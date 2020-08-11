using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Mirror;
using kobk.csharp.input;
using kobk.csharp.network;
using kobk.csharp.game.player.attack;

namespace kobk.csharp.game.player
{
    public abstract class GamePlayerCharacter : NetworkBehaviour
    {
        //Serialized Fields
        [Header("Objects")]
        [SerializeField] private CameraControl cameraControl = null;
        [SerializeField] private NetworkAnimator netAnimate = null;
        [Header("AnimationTriggers")]
        [SerializeField] private string DeathTrigger = "Death";
        [SerializeField] private string IsRunningBool = "Is Running";
        [SerializeField] private double DelayDeathBeforeDestroy = 5.0f;

        //Local Variables
        //public GamePlayerBase playerBase = null;
        [SyncVar]
        public int id = 0;

        private GameControls _controls = null;
        public GameControls controls
        {
            get
            {
                if (_controls == null)
                    _controls = new GameControls();
                return _controls;
            }
        }

        //Functionality Variables
        protected List<AtkAction> AttackTypeList = new List<AtkAction>();

        private int _SelectedAttack = 0;
        protected int SelectedAttack
        {
            get
            {
                return _SelectedAttack;
            }
            set
            {
                if (AttackTypeList.Count == 0)
                    _SelectedAttack = 0;
                else if (value < 0 || value >= AttackTypeList.Count)
                {
                    _SelectedAttack = 0;
                }
                else
                {
                    _SelectedAttack = value;
                }
            }
        }

        [SyncVar(hook = nameof(HandleHealthChange))]
        private int Health = 3;
        [SyncVar(hook = nameof(HandleHealthChange))]
        public int ammo = 3;

        private void HandleHealthChange(int oldVal, int newVal)
        {
            //TODO UI
        }

        private void HandleAmmoChange(int oldVal, int newVal) {
            //TODO UI
        }

        [ClientCallback]
        protected virtual void OnEnable()
        {
            controls.Player.AtkAction.performed += ctx => onAtkAction();
            controls.Player.ChangeWeapon.performed += ctx => onChangeAtk();

            NetworkManagerLobby room = NetworkManagerLobby.singleton as NetworkManagerLobby;

            room.gameCharacterListings.TryAdd(id, this);
        }

        [ClientCallback]
        protected virtual void OnDisable()
        {
            controls.Player.AtkAction.performed -= ctx => onAtkAction();
            controls.Player.ChangeWeapon.performed -= ctx => onChangeAtk();
        }

        protected event Action doUpdate;

        protected virtual void Update() {
            doUpdate?.Invoke();
        }

        [ServerCallback]
        protected virtual void OnDestroy()
        {
            if(!isDead)
                ServerHandleDeath(null);
        }

        [Client]
        private void onAtkAction()
        {
            CmdAtkAction(SelectedAttack);
        }

        [Command]
        private void CmdAtkAction(int SelectedAtk)
        {
            if (SelectedAtk < AttackTypeList.Count && SelectedAtk >= 0)
            {
                AttackTypeList[SelectedAtk].OnAction(this);
            }
        }

        [Client]
        private void onChangeAtk()
        {
            if (AttackTypeList.Count > 0)
            {
                AttackTypeList[SelectedAttack].OnDisable();
                SelectedAttack += 1;
                AttackTypeList[SelectedAttack].OnEnable();
            }
        }

        [Server]
        private void takeDamage(int v, GamePlayerCharacter Atker)
        {
            Health -= v;
            if (Health <= 0)
            {
                ServerHandleDeath(Atker);
            }
        }

        private double timeToDeath = -1.0f;
        private bool isDead = false;

        [Server]
        private void ServerHandleDeath(GamePlayerCharacter Atker)
        {
            isDead = true;
            NetworkManagerLobby room = (NetworkManagerLobby.singleton as NetworkManagerLobby);
            if (Atker is null)
            {
                RpcClientHandleDeath(new DeathData(-1, "You killed yourself"));
            }
            else
            {
                //killer name
                room.gamePlayerListings.TryToGetValue(Atker.id, out GamePlayerBase pb);
                string name = (pb is null) ? "Name not found" : pb.name;

                RpcClientHandleDeath(new DeathData(Atker.id, "You were killed by " + name));
            }
            netAnimate.SetTrigger(DeathTrigger);
            room.gameCharacterListings.Remove(this);
            timeToDeath = DelayDeathBeforeDestroy;
            doUpdate += updateDeath;
        }

        [Server]
        private void updateDeath() {
            timeToDeath -= Time.deltaTime;
            if(timeToDeath <= 0) {
                NetworkServer.Destroy(gameObject);
            }
        }

        [ClientRpc]
        private void RpcClientHandleDeath(DeathData data)
        {
            isDead = true;
            (NetworkManagerLobby.singleton as NetworkManagerLobby).gameCharacterListings.Remove(this);
            if (hasAuthority)
            {
                cameraControl.disableCam();
                //killTarget.enableCam();
                //Do Gui

            }
        }

    }
}