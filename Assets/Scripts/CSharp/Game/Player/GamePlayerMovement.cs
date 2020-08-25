using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using kobk.csharp.input;
using System;

namespace kobk.csharp.game.player
{
    public class GamePlayerMovement : NetworkBehaviour
    {
        [SerializeField] private float playerSpeed = 5f;
        [SerializeField] private CharacterController characterController = null;
        public event Action onMoveAttempt;

        private Vector2 prevMovement = Vector2.zero;
        
        [HideInInspector]
        [SyncVar]
        public bool MovementEnabled = true;

        private GameControls _controls = null;
        private GameControls controls {
            get{
                if(_controls == null) {
                    //_controls = controllable.getControlsObject();
                    if(TryGetComponent<GamePlayerCharacter>(out GamePlayerCharacter c)) {
                        _controls = c.controls;
                    } else {
                        throw new System.Exception();
                    }
                }
                return _controls;
            }
        }

        public override void OnStartAuthority() {
            enabled = true;

            controls.Player.Move.performed += ctx => CmdSetMovement(ctx.ReadValue<Vector2>());
            controls.Player.Move.canceled += ctx => CmdResetMovement();
        }

        [ClientCallback]
        private void OnEnable() => controls.Enable();
        [ClientCallback]
        private void onDisable() => controls.Disable();
        [ServerCallback]
        private void Update() => move();

        [Command]
        private void CmdSetMovement(Vector2 n) {
            onMoveAttempt?.Invoke();
            prevMovement = n.normalized;
        }

        [Command]
        private void CmdResetMovement() => prevMovement = Vector2.zero;

        [Server]
        private void move() {
            if (MovementEnabled)
            {
                Vector3 right = characterController.transform.right;
                Vector3 up = characterController.transform.up;

                right.z = 0f;
                up.z = 0f;

                Vector3 resultantMovement = right.normalized * prevMovement.x + up.normalized * prevMovement.y;

                characterController.Move(resultantMovement * playerSpeed * Time.deltaTime);
            }
        }

    }
}