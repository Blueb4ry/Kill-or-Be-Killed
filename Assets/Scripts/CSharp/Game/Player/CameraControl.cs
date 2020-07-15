using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Mirror;
using Cinemachine;
using kobk.csharp.input;

namespace kobk.csharp.game.player
{
    public class CameraControl : NetworkBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera virtualCam = null;
        [Header("Mouse Look")]
        [SerializeField] private bool enableMouseLook = false;
        [SerializeField] private Transform refObj = null;
        [SerializeField] [Range(0.0f, 0.5f)] private float HorizontalLookZonePercent = 0.33f;
        [SerializeField] [Range(0.0f, 0.5f)] private float VerticalLookZonePercent = 0.2f;
        [SerializeField] private float HorizontalLookDistance = 2.0f;
        [SerializeField] private float VerticalLookDistance = 1.0f;

        //Look around values
        private float Screen_LeftBounds = 0;
        private float Screen_RightBounds = 0;
        private float Screen_UpBounds = 0;
        private float Screen_DownBounds = 0;

        private bool isHoldingLookEnable = false;
        private Vector2 prevMousePos = Vector2.zero;

        //Look around event
        private event Action OnLookUpdate;

        private GameControls _controls = null;
        private GameControls controls
        {
            get
            {
                if (_controls == null)
                {
                    //_controls = controllable.getControlsObject();
                    if (TryGetComponent<GamePlayerCharacter>(out GamePlayerCharacter c))
                    {
                        _controls = c.controls;
                    }
                    else
                    {
                        throw new System.Exception();
                    }
                }
                return _controls;
            }
        }

        [ClientCallback]
        public override void OnStartAuthority()
        {
            virtualCam.enabled = true;
            if (enableMouseLook)
            {
                float width = Screen.width;

                Screen_LeftBounds = width * HorizontalLookZonePercent;
                Screen_RightBounds = width - Screen_LeftBounds;

                float height = Screen.height;

                Screen_UpBounds = height * VerticalLookZonePercent;
                Screen_DownBounds = height - Screen_UpBounds;

                OnLookUpdate += onMouseAction;

                controls.Player.EnableLook.started += ctx => isHoldingLookEnable = true;
                controls.Player.EnableLook.canceled += ctx => resetLookOff();
            }
        }

        private void resetLookOff() {
            isHoldingLookEnable = false;
            refObj.localPosition = Vector3.zero;
        }

        [ClientCallback]
        private void Update()
        {
            if (hasAuthority)
            {
                if (enableMouseLook)
                {
                    if(isHoldingLookEnable)
                        OnLookUpdate?.Invoke();
                }
            }
        }

        [Client]
        public void enableCam()
        {
            virtualCam.enabled = true;
        }

        [Client]
        public void disableCam()
        {
            virtualCam.enabled = false;
        }

        [Client]
        private void onMouseAction()
        {
            //Debug.Log(loc)l
            Vector2 loc = controls.Player.Look.ReadValue<Vector2>();

            if (!(loc == prevMousePos))
            {
                prevMousePos = loc;
                Vector3 newRefPos = new Vector3(0, 0, 0);
                if (loc.x < Screen_LeftBounds)
                {
                    //refObj.localPosition = new Vector3(-HorizontalLookDistance, 0, 0);
                    newRefPos.x = -HorizontalLookDistance;
                }
                else if (loc.x > Screen_RightBounds)
                {
                    //refObj.localPosition = new Vector3(HorizontalLookDistance, 0, 0);
                    newRefPos.x = HorizontalLookDistance;
                }
                // else
                // {
                //     refObj.localPosition = Vector3.zero;
                // }

                if (loc.y < Screen_UpBounds)
                {
                    //refObj.localPosition = new Vector3(-HorizontalLookDistance, 0, 0);
                    newRefPos.y = -VerticalLookDistance;
                }
                else if (loc.y > Screen_DownBounds)
                {
                    //refObj.localPosition = new Vector3(HorizontalLookDistance, 0, 0);
                    newRefPos.y = VerticalLookDistance;
                }

                refObj.localPosition = newRefPos;

            }
        }

    }
}