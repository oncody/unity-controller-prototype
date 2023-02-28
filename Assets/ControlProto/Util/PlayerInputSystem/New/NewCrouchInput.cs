using UnityEngine.InputSystem;

namespace ControlProto.Util.PlayerInputSystem.New {
    public class NewCrouchInput : IPlayerToggleInput {
        private readonly InputAction crouchAction;
        private bool isCrouchButtonHeldDown;

        public NewCrouchInput() {
            crouchAction = new InputAction("Crouch", InputActionType.Value, "<Keyboard>/leftCtrl");
            crouchAction.started += CrouchStartedCallback;
            crouchAction.canceled += CrouchCanceledCallback;
            crouchAction.Enable();
        }

        private void CrouchStartedCallback(InputAction.CallbackContext context) {
            isCrouchButtonHeldDown = true;
        }

        private void CrouchCanceledCallback(InputAction.CallbackContext context) {
            isCrouchButtonHeldDown = false;
        }

        public bool IsHeldDown() {
            return isCrouchButtonHeldDown;
        }
    }
}
