using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using kobk.csharp.game.roomObjects;

namespace kobk.csharp.game.player
{
    public class GameNinjaCharacter : GamePlayerCharacter
    {
        //Serialized Fields
        [SerializeField] private GamePlayerMovement playerMovement = null;
        [SerializeField] private SpriteRenderer sprite = null;
        [SerializeField] private float maxDistanceHideJump = 20.0f;

        //Functional Variables
        [SyncVar(hook = nameof(OnHideChange))]
        public bool isHiding = false;

        private Vector3 prevWorldPosHideCheck = default(Vector2);
        private Camera cam = null;
        private int curSelHiding = -1;

        protected override void OnEnable()
        {
            base.OnEnable();

            //Add attacks


            //add controls
            if (!isServer)
            {
                controls.Player.Hide.performed += ctx => CmdHideAction(curSelHiding);
                doUpdate += HideCheck;
                cam = Camera.main;
            }
        }

        public void OnHideChange(bool oldVal, bool newVal) => HideCheck();

        [Client]
        public void HideCheck()
        {
            Vector2 screenpos = controls.Player.Look.ReadValue<Vector2>();

            Vector3 curPos = cam.ScreenToWorldPoint(screenpos);

            if(curPos != prevWorldPosHideCheck)
			{
                prevWorldPosHideCheck = curPos;

                if(curSelHiding != -1)
				{
                    HidingObjectManager.instance.hidingObjects[curSelHiding].setPop(false);
                    curSelHiding = -1;
				}

                if (Vector2.Distance(curPos, transform.position) <= maxDistanceHideJump)
                {
                    Collider2D found = Physics2D.OverlapPoint(curPos, 8);

                    if (found is null)
                        return;

                    HidingObject hideObj = found.GetComponentInParent<HidingObject>();

                    if (!(hideObj is null))
                    {
                        Debug.Log("set");
                        hideObj.setPop(true);
                        curSelHiding = hideObj.networkhideId;
                    }
                }
			}
        }

        private void onHide(bool oldVal, bool newVal) {
            sprite.enabled = !newVal;
        }

        [Command]
        public void CmdHideAction(int obj)
        {
            playerMovement.enabled = false;
        }
    }
}