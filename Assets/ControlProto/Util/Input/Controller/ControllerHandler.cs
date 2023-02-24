namespace ControlProto.Util.Input.Controller {
    public class ControllerHandler {
        public float HorizontalMouseMovement() {
            return ResponsiveValue(ControllerInput.HorizontalMouseMovement);
        }

        public float VerticalMouseMovement() {
            return ResponsiveValue(ControllerInput.VerticalMouseMovement);
        }

        public float HorizontalKeyboardMovement() {
            return ResponsiveValue(ControllerInput.HorizontalKeyboardMovement);
        }

        public float VerticalKeyboardMovement() {
            return ResponsiveValue(ControllerInput.VerticalKeyboardMovement);
        }

        /**
         * Use this to get a joystick or mouse value. This returns a range.
         */
        private float SmoothValue(ControllerInput input) {
            return UnityEngine.Input.GetAxis(ControllerInputMapper.GetMapping(input));
        }

        /**
         * Use this to get a key press value. This returns a binary one or the other
         */
        private float ResponsiveValue(ControllerInput input) {
            return UnityEngine.Input.GetAxisRaw(ControllerInputMapper.GetMapping(input));
        }
    }
}
