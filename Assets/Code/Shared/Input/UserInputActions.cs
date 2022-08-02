//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/Code/Shared/Input/UserInputActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Framework.Shared.Input
{
    public partial class @UserInputActions : IInputActionCollection2, IDisposable
    {
        public InputActionAsset asset { get; }
        public @UserInputActions()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""UserInputActions"",
    ""maps"": [
        {
            ""name"": ""Interactions"",
            ""id"": ""725c6427-de6e-4e00-8b6e-35dc133e74e3"",
            ""actions"": [
                {
                    ""name"": ""Tap"",
                    ""type"": ""Button"",
                    ""id"": ""b19b0bfc-af01-400e-b1b3-a02823bfbf4d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Tap"",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""b7bfe185-1ed1-44db-969a-314389b19c78"",
                    ""path"": ""<Pointer>/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Tap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // Interactions
            m_Interactions = asset.FindActionMap("Interactions", throwIfNotFound: true);
            m_Interactions_Tap = m_Interactions.FindAction("Tap", throwIfNotFound: true);
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
        public IEnumerable<InputBinding> bindings => asset.bindings;

        public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
        {
            return asset.FindAction(actionNameOrId, throwIfNotFound);
        }
        public int FindBinding(InputBinding bindingMask, out InputAction action)
        {
            return asset.FindBinding(bindingMask, out action);
        }

        // Interactions
        private readonly InputActionMap m_Interactions;
        private IInteractionsActions m_InteractionsActionsCallbackInterface;
        private readonly InputAction m_Interactions_Tap;
        public struct InteractionsActions
        {
            private @UserInputActions m_Wrapper;
            public InteractionsActions(@UserInputActions wrapper) { m_Wrapper = wrapper; }
            public InputAction @Tap => m_Wrapper.m_Interactions_Tap;
            public InputActionMap Get() { return m_Wrapper.m_Interactions; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(InteractionsActions set) { return set.Get(); }
            public void SetCallbacks(IInteractionsActions instance)
            {
                if (m_Wrapper.m_InteractionsActionsCallbackInterface != null)
                {
                    @Tap.started -= m_Wrapper.m_InteractionsActionsCallbackInterface.OnTap;
                    @Tap.performed -= m_Wrapper.m_InteractionsActionsCallbackInterface.OnTap;
                    @Tap.canceled -= m_Wrapper.m_InteractionsActionsCallbackInterface.OnTap;
                }
                m_Wrapper.m_InteractionsActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Tap.started += instance.OnTap;
                    @Tap.performed += instance.OnTap;
                    @Tap.canceled += instance.OnTap;
                }
            }
        }
        public InteractionsActions @Interactions => new InteractionsActions(this);
        public interface IInteractionsActions
        {
            void OnTap(InputAction.CallbackContext context);
        }
    }
}
