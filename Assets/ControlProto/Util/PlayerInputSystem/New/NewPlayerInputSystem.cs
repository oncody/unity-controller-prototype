using UnityEngine;
using UnityEngine.InputSystem;

namespace ControlProto.Util.PlayerInputSystem.New {
    public class NewPlayerInputSystem : IPlayerInputSystem {
        private readonly DefaultInputActions defaultInputActions;
        private Vector2 lookInput = Vector2.zero;
        private Vector2 moveInput = Vector2.zero;
        private bool isCrouchButtonHeldDown;
        private bool isSprintButtonHeldDown;

        public NewPlayerInputSystem(DefaultInputActions defaultInputActions) {
            this.defaultInputActions = defaultInputActions;
            InitializeLookInput();
            InitializeMoveInput();
            InitializeCrouchInput();
            InitializeSprintInput();
            defaultInputActions.Enable();
        }

        private void InitializeLookInput() {
            defaultInputActions.Player.Look.performed += context => { lookInput = context.ReadValue<Vector2>(); };

            defaultInputActions.Player.Look.canceled += _ => lookInput = Vector2.zero;
        }

        private void InitializeMoveInput() {
            defaultInputActions.Player.Move.performed += context => moveInput = context.ReadValue<Vector2>();
            defaultInputActions.Player.Move.canceled += _ => moveInput = Vector2.zero;
        }

        private void InitializeCrouchInput() {
            InputAction crouchAction = new InputAction("Crouch", InputActionType.Value, "<Keyboard>/leftCtrl");
            crouchAction.started += _ => isCrouchButtonHeldDown = true;
            crouchAction.canceled += _ => isCrouchButtonHeldDown = false;
            crouchAction.Enable();
        }

        private void InitializeSprintInput() {
            InputAction sprintAction = new InputAction("Sprint", InputActionType.Value, "<Keyboard>/leftShift");
            sprintAction.started += _ => isSprintButtonHeldDown = true;
            sprintAction.canceled += _ => isSprintButtonHeldDown = false;
            sprintAction.Enable();
        }

        public float HorizontalLookInput() {
            return lookInput.x;
        }

        public float VerticalLookInput() {
            return lookInput.y;
        }

        public float HorizontalMoveInput() {
            return moveInput.x;
        }

        public float VerticalLMoveInput() {
            return moveInput.y;
        }

        public bool CrouchInput() {
            return isCrouchButtonHeldDown;
        }

        public bool SprintInput() {
            return isSprintButtonHeldDown;
        }
    }
}
