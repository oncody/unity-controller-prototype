using UnityEngine.InputSystem;

namespace ControlProto.Util.Input.Controller {
    public class ControllerHandler {
        private readonly Mouse mouse;
        private readonly UnityEngine.InputSystem.Keyboard keyboard;

        public ControllerHandler(Mouse mouse, UnityEngine.InputSystem.Keyboard keyboard) {
            this.mouse = mouse;
            this.keyboard = keyboard;
        }

        public float HorizontalMouseMovement() {
            return mouse.delta.x.ReadValue();
        }

        public float VerticalMouseMovement() {
            return mouse.delta.y.ReadValue();
        }

        public float HorizontalKeyboardMovement() {
            return keyboard.dKey.ReadValue() - keyboard.aKey.ReadValue();
        }

        public float VerticalKeyboardMovement() {
            return keyboard.wKey.ReadValue() - keyboard.sKey.ReadValue();
        }
    }
}
