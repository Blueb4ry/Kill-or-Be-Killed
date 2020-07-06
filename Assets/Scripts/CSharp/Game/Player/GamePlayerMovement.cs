using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using kobk.csharp.input;

namespace kobk.csharp.game.player
{
    public class GamePlayerMovement : NetworkBehaviour
    {
        [SerializeField] private float playerSpeed = 5f;
        [SerializeField] private CharacterController characterController;

        private Vector2 prevMovement = Vector2.zero;

        private GameControls _controls;
        private GameControls controls {
            get{
                if(_controls == null)
                    _controls = new GameControls();
                return _controls;
            }
        }

        public override void OnStartAuthority() {
            enabled = true;

            controls.Player.Move.performed += ctx => setMovement(ctx.ReadValue<Vector2>());
            controls.Player.Move.canceled += ctx => resetMovement();
        }

        [ClientCallback]
        private void OnEnable() => controls.Enable();
        [ClientCallback]
        private void onDisable() => controls.Disable();
        [ClientCallback]
        private void Update() => move();

        [Client]
        private void setMovement(Vector2 n) => prevMovement = n.normalized;
        [Client]
        private void resetMovement() => prevMovement = Vector2.zero;

        [Client]
        private void move() {
            Vector3 right = characterController.transform.right;
            Vector3 up = characterController.transform.up;

            right.z = 0f;
            up.z = 0f;

            Vector3 resultantMovement = right.normalized * prevMovement.x + up.normalized * prevMovement.y;

            characterController.Move(resultantMovement * playerSpeed * Time.deltaTime);
        }

    }
}