// GENERATED AUTOMATICALLY FROM 'Assets/Input/GameControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace kobk.csharp.input
{
    public class @GameControls : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @GameControls()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""GameControls"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""93cb20b2-147b-4dc0-bbfc-cfa754641b73"",
            ""actions"": [
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""8c31d6ed-1636-465f-adc5-8d495c306fce"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""0ec06ae0-4860-41cd-b676-c111f0aaf4f6"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""e1a648f4-0f8b-49ef-88cf-f8d8cb33b260"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""AtkAction"",
                    ""type"": ""Button"",
                    ""id"": ""63745277-d43c-4b2b-9273-e99fa33b00f0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ChangeWeapon"",
                    ""type"": ""Button"",
                    ""id"": ""c2f6147c-e0d9-480a-bb41-ec72b705b5b6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Hide"",
                    ""type"": ""Button"",
                    ""id"": ""e0c30612-751a-4b50-a0f6-67fc9513d65b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""e5129093-7163-45b6-bff1-80ead429a723"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""ea2c8891-17e7-4934-b2aa-e1f75e09ec2c"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""09913ec2-2d12-483d-8dcf-a9c99497dd78"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""324d132d-1e80-44d3-a0b4-69d48ac3cab8"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""3fca8788-749a-488d-a035-3d118309c0be"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""0822b5b8-f88a-4e1b-b9d2-3a7370220dfc"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8c4c4277-0a43-4f9e-b9a6-053e4e15ebfc"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""AtkAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""95c6134a-e183-420a-8076-a68f93e1e033"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""ChangeWeapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4b6f29ab-a0a1-4588-a153-8753aa76a56b"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Hide"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""da2a32be-a53e-437d-9a3a-7b3541624a54"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""bfc2753e-2954-4e43-9dff-4d3850f82625"",
            ""actions"": [
                {
                    ""name"": ""TabAction"",
                    ""type"": ""Button"",
                    ""id"": ""dac1a420-ac86-45b8-888d-f4fae0d3ca42"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""EnterAction"",
                    ""type"": ""Button"",
                    ""id"": ""d9e227d8-3873-4af2-8618-19c112f31e7b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""BackAction"",
                    ""type"": ""Button"",
                    ""id"": ""69196600-e1a1-4805-b39f-a81c310ae257"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""d9be4e7f-8111-4029-822c-ab892b2555d4"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""TabAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c939d5cc-90e4-42f1-ba59-a370049c30f2"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""EnterAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4a35d35e-d418-4770-878a-17b3079d990a"",
                    ""path"": ""<Keyboard>/numpadEnter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""EnterAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""55c1ade5-d57d-4128-81b1-9db67469ce03"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""BackAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""11fecbb7-ef07-4284-81ee-9c3d72defdc2"",
                    ""path"": ""<Keyboard>/backspace"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""BackAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard & Mouse"",
            ""bindingGroup"": ""Keyboard & Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
            // Player
            m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
            m_Player_Pause = m_Player.FindAction("Pause", throwIfNotFound: true);
            m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
            m_Player_Look = m_Player.FindAction("Look", throwIfNotFound: true);
            m_Player_AtkAction = m_Player.FindAction("AtkAction", throwIfNotFound: true);
            m_Player_ChangeWeapon = m_Player.FindAction("ChangeWeapon", throwIfNotFound: true);
            m_Player_Hide = m_Player.FindAction("Hide", throwIfNotFound: true);
            // UI
            m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
            m_UI_TabAction = m_UI.FindAction("TabAction", throwIfNotFound: true);
            m_UI_EnterAction = m_UI.FindAction("EnterAction", throwIfNotFound: true);
            m_UI_BackAction = m_UI.FindAction("BackAction", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }

        // Player
        private readonly InputActionMap m_Player;
        private IPlayerActions m_PlayerActionsCallbackInterface;
        private readonly InputAction m_Player_Pause;
        private readonly InputAction m_Player_Move;
        private readonly InputAction m_Player_Look;
        private readonly InputAction m_Player_AtkAction;
        private readonly InputAction m_Player_ChangeWeapon;
        private readonly InputAction m_Player_Hide;
        public struct PlayerActions
        {
            private @GameControls m_Wrapper;
            public PlayerActions(@GameControls wrapper) { m_Wrapper = wrapper; }
            public InputAction @Pause => m_Wrapper.m_Player_Pause;
            public InputAction @Move => m_Wrapper.m_Player_Move;
            public InputAction @Look => m_Wrapper.m_Player_Look;
            public InputAction @AtkAction => m_Wrapper.m_Player_AtkAction;
            public InputAction @ChangeWeapon => m_Wrapper.m_Player_ChangeWeapon;
            public InputAction @Hide => m_Wrapper.m_Player_Hide;
            public InputActionMap Get() { return m_Wrapper.m_Player; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
            public void SetCallbacks(IPlayerActions instance)
            {
                if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
                {
                    @Pause.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPause;
                    @Pause.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPause;
                    @Pause.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPause;
                    @Move.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                    @Move.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                    @Move.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                    @Look.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
                    @Look.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
                    @Look.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
                    @AtkAction.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAtkAction;
                    @AtkAction.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAtkAction;
                    @AtkAction.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAtkAction;
                    @ChangeWeapon.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnChangeWeapon;
                    @ChangeWeapon.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnChangeWeapon;
                    @ChangeWeapon.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnChangeWeapon;
                    @Hide.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnHide;
                    @Hide.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnHide;
                    @Hide.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnHide;
                }
                m_Wrapper.m_PlayerActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Pause.started += instance.OnPause;
                    @Pause.performed += instance.OnPause;
                    @Pause.canceled += instance.OnPause;
                    @Move.started += instance.OnMove;
                    @Move.performed += instance.OnMove;
                    @Move.canceled += instance.OnMove;
                    @Look.started += instance.OnLook;
                    @Look.performed += instance.OnLook;
                    @Look.canceled += instance.OnLook;
                    @AtkAction.started += instance.OnAtkAction;
                    @AtkAction.performed += instance.OnAtkAction;
                    @AtkAction.canceled += instance.OnAtkAction;
                    @ChangeWeapon.started += instance.OnChangeWeapon;
                    @ChangeWeapon.performed += instance.OnChangeWeapon;
                    @ChangeWeapon.canceled += instance.OnChangeWeapon;
                    @Hide.started += instance.OnHide;
                    @Hide.performed += instance.OnHide;
                    @Hide.canceled += instance.OnHide;
                }
            }
        }
        public PlayerActions @Player => new PlayerActions(this);

        // UI
        private readonly InputActionMap m_UI;
        private IUIActions m_UIActionsCallbackInterface;
        private readonly InputAction m_UI_TabAction;
        private readonly InputAction m_UI_EnterAction;
        private readonly InputAction m_UI_BackAction;
        public struct UIActions
        {
            private @GameControls m_Wrapper;
            public UIActions(@GameControls wrapper) { m_Wrapper = wrapper; }
            public InputAction @TabAction => m_Wrapper.m_UI_TabAction;
            public InputAction @EnterAction => m_Wrapper.m_UI_EnterAction;
            public InputAction @BackAction => m_Wrapper.m_UI_BackAction;
            public InputActionMap Get() { return m_Wrapper.m_UI; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
            public void SetCallbacks(IUIActions instance)
            {
                if (m_Wrapper.m_UIActionsCallbackInterface != null)
                {
                    @TabAction.started -= m_Wrapper.m_UIActionsCallbackInterface.OnTabAction;
                    @TabAction.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnTabAction;
                    @TabAction.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnTabAction;
                    @EnterAction.started -= m_Wrapper.m_UIActionsCallbackInterface.OnEnterAction;
                    @EnterAction.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnEnterAction;
                    @EnterAction.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnEnterAction;
                    @BackAction.started -= m_Wrapper.m_UIActionsCallbackInterface.OnBackAction;
                    @BackAction.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnBackAction;
                    @BackAction.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnBackAction;
                }
                m_Wrapper.m_UIActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @TabAction.started += instance.OnTabAction;
                    @TabAction.performed += instance.OnTabAction;
                    @TabAction.canceled += instance.OnTabAction;
                    @EnterAction.started += instance.OnEnterAction;
                    @EnterAction.performed += instance.OnEnterAction;
                    @EnterAction.canceled += instance.OnEnterAction;
                    @BackAction.started += instance.OnBackAction;
                    @BackAction.performed += instance.OnBackAction;
                    @BackAction.canceled += instance.OnBackAction;
                }
            }
        }
        public UIActions @UI => new UIActions(this);
        private int m_KeyboardMouseSchemeIndex = -1;
        public InputControlScheme KeyboardMouseScheme
        {
            get
            {
                if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard & Mouse");
                return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
            }
        }
        public interface IPlayerActions
        {
            void OnPause(InputAction.CallbackContext context);
            void OnMove(InputAction.CallbackContext context);
            void OnLook(InputAction.CallbackContext context);
            void OnAtkAction(InputAction.CallbackContext context);
            void OnChangeWeapon(InputAction.CallbackContext context);
            void OnHide(InputAction.CallbackContext context);
        }
        public interface IUIActions
        {
            void OnTabAction(InputAction.CallbackContext context);
            void OnEnterAction(InputAction.CallbackContext context);
            void OnBackAction(InputAction.CallbackContext context);
        }
    }
}
