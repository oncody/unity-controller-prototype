using UnityEngine.InputSystem;

namespace ControlProto.Util.PlayerInputSystem.New {
    public class NewSprintInput : IPlayerToggleInput {
        private readonly InputAction sprintAction;
        private bool isSprintButtonHeldDown;

        public NewSprintInput() {
            sprintAction = new InputAction("Sprint", InputActionType.Value, "<Keyboard>/leftShift");
            sprintAction.started += SprintStartedCallback;
            sprintAction.canceled += SprintCanceledCallback;
            sprintAction.Enable();
        }

        private void SprintStartedCallback(InputAction.CallbackContext context) {
            isSprintButtonHeldDown = true;
        }

        private void SprintCanceledCallback(InputAction.CallbackContext context) {
            isSprintButtonHeldDown = false;
        }

        public bool IsHeldDown() {
            return isSprintButtonHeldDown;
        }
    }
}
