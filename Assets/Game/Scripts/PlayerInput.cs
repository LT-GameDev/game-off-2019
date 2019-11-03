// GENERATED AUTOMATICALLY FROM 'Assets/Game/Settings/PlayerInput.inputactions'

using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Game
{
    public class PlayerInput : IInputActionCollection
    {
        private InputActionAsset asset;
        public PlayerInput()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInput"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""2156985d-7ca5-4f55-ad12-69e24dacca2b"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""1a4d406e-32bf-49d0-9994-e29c83d70e80"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ToggleWalkRun"",
                    ""type"": ""Button"",
                    ""id"": ""85df6321-66ad-4cd1-ad5d-24ca723d0364"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Sprint"",
                    ""type"": ""Button"",
                    ""id"": ""a173dd45-df40-4ef5-b734-5f253041b006"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""a039e373-28df-43a0-bda1-05f366195c98"",
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
                    ""id"": ""1950458e-6591-42ec-bc54-2ec4cd281a14"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""8e40bd66-6b9c-4c85-a6b6-ca4a6a894744"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""d0610856-5413-41c0-a78e-0406cd610b24"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""85a75804-dce7-4b62-92fd-c894d7d07d18"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""64f97e21-a423-4850-a148-7892293da2b3"",
                    ""path"": ""<Keyboard>/capsLock"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""ToggleWalkRun"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d5ce28ab-71aa-4251-9f3f-d20ccb71c2d4"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard&Mouse"",
            ""bindingGroup"": ""Keyboard&Mouse"",
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
            m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
            m_Player_ToggleWalkRun = m_Player.FindAction("ToggleWalkRun", throwIfNotFound: true);
            m_Player_Sprint = m_Player.FindAction("Sprint", throwIfNotFound: true);
        }

        ~PlayerInput()
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
        private readonly InputAction m_Player_Move;
        private readonly InputAction m_Player_ToggleWalkRun;
        private readonly InputAction m_Player_Sprint;
        public struct PlayerActions
        {
            private PlayerInput m_Wrapper;
            public PlayerActions(PlayerInput wrapper) { m_Wrapper = wrapper; }
            public InputAction @Move => m_Wrapper.m_Player_Move;
            public InputAction @ToggleWalkRun => m_Wrapper.m_Player_ToggleWalkRun;
            public InputAction @Sprint => m_Wrapper.m_Player_Sprint;
            public InputActionMap Get() { return m_Wrapper.m_Player; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
            public void SetCallbacks(IPlayerActions instance)
            {
                if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
                {
                    Move.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                    Move.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                    Move.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                    ToggleWalkRun.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnToggleWalkRun;
                    ToggleWalkRun.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnToggleWalkRun;
                    ToggleWalkRun.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnToggleWalkRun;
                    Sprint.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSprint;
                    Sprint.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSprint;
                    Sprint.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSprint;
                }
                m_Wrapper.m_PlayerActionsCallbackInterface = instance;
                if (instance != null)
                {
                    Move.started += instance.OnMove;
                    Move.performed += instance.OnMove;
                    Move.canceled += instance.OnMove;
                    ToggleWalkRun.started += instance.OnToggleWalkRun;
                    ToggleWalkRun.performed += instance.OnToggleWalkRun;
                    ToggleWalkRun.canceled += instance.OnToggleWalkRun;
                    Sprint.started += instance.OnSprint;
                    Sprint.performed += instance.OnSprint;
                    Sprint.canceled += instance.OnSprint;
                }
            }
        }
        public PlayerActions @Player => new PlayerActions(this);
        private int m_KeyboardMouseSchemeIndex = -1;
        public InputControlScheme KeyboardMouseScheme
        {
            get
            {
                if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard&Mouse");
                return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
            }
        }
        public interface IPlayerActions
        {
            void OnMove(InputAction.CallbackContext context);
            void OnToggleWalkRun(InputAction.CallbackContext context);
            void OnSprint(InputAction.CallbackContext context);
        }
    }
}
