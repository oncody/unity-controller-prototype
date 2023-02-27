using UnityEngine;
using UnityEngine.InputSystem;

namespace ControlProto.Util {
    public class CursorManager {
        private readonly InputAction exitFocusAction;

        public CursorManager() {
            LockCursor();

            exitFocusAction = new InputAction("ExitFocus", InputActionType.Button, "<Keyboard>/escape");
            exitFocusAction.performed += UnlockCursor;
            exitFocusAction.Enable();
        }

        private void LockCursor() {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void UnlockCursor(InputAction.CallbackContext context) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
