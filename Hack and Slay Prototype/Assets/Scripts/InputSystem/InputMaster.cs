// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/InputSystem/InputMaster.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputMaster : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputMaster()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputMaster"",
    ""maps"": [
        {
            ""name"": ""Ingame"",
            ""id"": ""d3dec976-fc4a-4802-a220-bffb71c2d85d"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Button"",
                    ""id"": ""09d58b88-750b-4732-bbd0-691252cbaa65"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""40d3894d-d1ca-44c8-af88-ae5a18108799"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""3b38753a-8458-4f10-a81f-f87b4ee1b0c3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Crouch"",
                    ""type"": ""Button"",
                    ""id"": ""dc832db9-d0aa-449b-8b8a-0775f99fcad1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""e74130ac-36de-4138-b5e6-0047c47a6ed8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""2673efbc-a22a-4d88-a168-b1ecc82fccbf"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Throw"",
                    ""type"": ""Button"",
                    ""id"": ""19ac79e0-8810-415d-abcd-a5d38a1bd624"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Drop"",
                    ""type"": ""Button"",
                    ""id"": ""6f050fab-aa19-458d-a32c-bdc405d3990c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""AD-Controll"",
                    ""id"": ""4250ad22-1118-48c7-9f31-c94718114a9b"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""eb71221e-9aab-4ceb-a60c-1cd3c59647da"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse and Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""b14606e9-1d65-4d51-993f-c9a3433ccc7a"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse and Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""66e1029b-c3aa-4817-9017-c01974f8e808"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse and Keyboard"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""41fd57c9-152f-4242-93e4-41cfbe54c3fc"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse and Keyboard"",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8462f7e6-7eba-40b6-9252-8d8b574fb35c"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse and Keyboard"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e3ae1a8d-7f85-41b2-a811-05e9738c851a"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse and Keyboard"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""db748cfe-b3ba-4922-80f5-ca819aa678f7"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse and Keyboard"",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bad12a1e-2c21-4eb9-819d-1594c5e49568"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse and Keyboard"",
                    ""action"": ""Throw"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""44d42d05-be74-4945-8ffa-11976b03c120"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse and Keyboard"",
                    ""action"": ""Drop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Mouse and Keyboard"",
            ""bindingGroup"": ""Mouse and Keyboard"",
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
        // Ingame
        m_Ingame = asset.FindActionMap("Ingame", throwIfNotFound: true);
        m_Ingame_Movement = m_Ingame.FindAction("Movement", throwIfNotFound: true);
        m_Ingame_Jump = m_Ingame.FindAction("Jump", throwIfNotFound: true);
        m_Ingame_Dash = m_Ingame.FindAction("Dash", throwIfNotFound: true);
        m_Ingame_Crouch = m_Ingame.FindAction("Crouch", throwIfNotFound: true);
        m_Ingame_Attack = m_Ingame.FindAction("Attack", throwIfNotFound: true);
        m_Ingame_Shoot = m_Ingame.FindAction("Shoot", throwIfNotFound: true);
        m_Ingame_Throw = m_Ingame.FindAction("Throw", throwIfNotFound: true);
        m_Ingame_Drop = m_Ingame.FindAction("Drop", throwIfNotFound: true);
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

    // Ingame
    private readonly InputActionMap m_Ingame;
    private IIngameActions m_IngameActionsCallbackInterface;
    private readonly InputAction m_Ingame_Movement;
    private readonly InputAction m_Ingame_Jump;
    private readonly InputAction m_Ingame_Dash;
    private readonly InputAction m_Ingame_Crouch;
    private readonly InputAction m_Ingame_Attack;
    private readonly InputAction m_Ingame_Shoot;
    private readonly InputAction m_Ingame_Throw;
    private readonly InputAction m_Ingame_Drop;
    public struct IngameActions
    {
        private @InputMaster m_Wrapper;
        public IngameActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Ingame_Movement;
        public InputAction @Jump => m_Wrapper.m_Ingame_Jump;
        public InputAction @Dash => m_Wrapper.m_Ingame_Dash;
        public InputAction @Crouch => m_Wrapper.m_Ingame_Crouch;
        public InputAction @Attack => m_Wrapper.m_Ingame_Attack;
        public InputAction @Shoot => m_Wrapper.m_Ingame_Shoot;
        public InputAction @Throw => m_Wrapper.m_Ingame_Throw;
        public InputAction @Drop => m_Wrapper.m_Ingame_Drop;
        public InputActionMap Get() { return m_Wrapper.m_Ingame; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(IngameActions set) { return set.Get(); }
        public void SetCallbacks(IIngameActions instance)
        {
            if (m_Wrapper.m_IngameActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_IngameActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_IngameActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_IngameActionsCallbackInterface.OnMovement;
                @Jump.started -= m_Wrapper.m_IngameActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_IngameActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_IngameActionsCallbackInterface.OnJump;
                @Dash.started -= m_Wrapper.m_IngameActionsCallbackInterface.OnDash;
                @Dash.performed -= m_Wrapper.m_IngameActionsCallbackInterface.OnDash;
                @Dash.canceled -= m_Wrapper.m_IngameActionsCallbackInterface.OnDash;
                @Crouch.started -= m_Wrapper.m_IngameActionsCallbackInterface.OnCrouch;
                @Crouch.performed -= m_Wrapper.m_IngameActionsCallbackInterface.OnCrouch;
                @Crouch.canceled -= m_Wrapper.m_IngameActionsCallbackInterface.OnCrouch;
                @Attack.started -= m_Wrapper.m_IngameActionsCallbackInterface.OnAttack;
                @Attack.performed -= m_Wrapper.m_IngameActionsCallbackInterface.OnAttack;
                @Attack.canceled -= m_Wrapper.m_IngameActionsCallbackInterface.OnAttack;
                @Shoot.started -= m_Wrapper.m_IngameActionsCallbackInterface.OnShoot;
                @Shoot.performed -= m_Wrapper.m_IngameActionsCallbackInterface.OnShoot;
                @Shoot.canceled -= m_Wrapper.m_IngameActionsCallbackInterface.OnShoot;
                @Throw.started -= m_Wrapper.m_IngameActionsCallbackInterface.OnThrow;
                @Throw.performed -= m_Wrapper.m_IngameActionsCallbackInterface.OnThrow;
                @Throw.canceled -= m_Wrapper.m_IngameActionsCallbackInterface.OnThrow;
                @Drop.started -= m_Wrapper.m_IngameActionsCallbackInterface.OnDrop;
                @Drop.performed -= m_Wrapper.m_IngameActionsCallbackInterface.OnDrop;
                @Drop.canceled -= m_Wrapper.m_IngameActionsCallbackInterface.OnDrop;
            }
            m_Wrapper.m_IngameActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Dash.started += instance.OnDash;
                @Dash.performed += instance.OnDash;
                @Dash.canceled += instance.OnDash;
                @Crouch.started += instance.OnCrouch;
                @Crouch.performed += instance.OnCrouch;
                @Crouch.canceled += instance.OnCrouch;
                @Attack.started += instance.OnAttack;
                @Attack.performed += instance.OnAttack;
                @Attack.canceled += instance.OnAttack;
                @Shoot.started += instance.OnShoot;
                @Shoot.performed += instance.OnShoot;
                @Shoot.canceled += instance.OnShoot;
                @Throw.started += instance.OnThrow;
                @Throw.performed += instance.OnThrow;
                @Throw.canceled += instance.OnThrow;
                @Drop.started += instance.OnDrop;
                @Drop.performed += instance.OnDrop;
                @Drop.canceled += instance.OnDrop;
            }
        }
    }
    public IngameActions @Ingame => new IngameActions(this);
    private int m_MouseandKeyboardSchemeIndex = -1;
    public InputControlScheme MouseandKeyboardScheme
    {
        get
        {
            if (m_MouseandKeyboardSchemeIndex == -1) m_MouseandKeyboardSchemeIndex = asset.FindControlSchemeIndex("Mouse and Keyboard");
            return asset.controlSchemes[m_MouseandKeyboardSchemeIndex];
        }
    }
    public interface IIngameActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnCrouch(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnShoot(InputAction.CallbackContext context);
        void OnThrow(InputAction.CallbackContext context);
        void OnDrop(InputAction.CallbackContext context);
    }
}
