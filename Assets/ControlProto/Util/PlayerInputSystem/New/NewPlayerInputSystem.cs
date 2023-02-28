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
            defaultInputActions.Player.Look.performed += context => lookInput = context.ReadValue<Vector2>();
            defaultInputActions.Player.Look.canceled += _ => lookInput = Vector2.zero;
        }

        private void InitializeMoveInput() {
            defaultInputActions.Player.Move.performed += context => moveInput = context.ReadValue<Vector2>();
            defaultInputActions.Player.Move.canceled += _ => moveInput = Vector2.zero;
        }

        private void InitializeCrouchInput() {
            InputAction crouchAction = new InputAction("Crouch", InputActionType.Value, "<Keyboard>/leftCtrl");
            crouchAction.started += CrouchStartedCallback;
            crouchAction.canceled += CrouchCanceledCallback;
            crouchAction.Enable();
        }

        private void InitializeSprintInput() {
            InputAction sprintAction = new InputAction("Sprint", InputActionType.Value, "<Keyboard>/leftShift");
            sprintAction.started += SprintStartedCallback;
            sprintAction.canceled += SprintCanceledCallback;
            sprintAction.Enable();
        }

        private void CrouchStartedCallback(InputAction.CallbackContext context) {
            isCrouchButtonHeldDown = true;
        }

        private void CrouchCanceledCallback(InputAction.CallbackContext context) {
            isCrouchButtonHeldDown = false;
        }

        private void SprintStartedCallback(InputAction.CallbackContext context) {
            isSprintButtonHeldDown = true;
        }

        private void SprintCanceledCallback(InputAction.CallbackContext context) {
            isSprintButtonHeldDown = false;
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
