namespace ControlProto.Util.Input.Controller {
    public class ControllerHandler {
        public float HorizontalMouseMovement() {
            return SmoothValue(ControllerInput.HorizontalMouseMovement);
        }

        public float VerticalMouseMovement() {
            return SmoothValue(ControllerInput.VerticalMouseMovement);
        }

        public float HorizontalKeyboardMovement() {
            return SmoothValue(ControllerInput.HorizontalKeyboardMovement);
        }

        public float VerticalKeyboardMovement() {
            return SmoothValue(ControllerInput.VerticalKeyboardMovement);
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
            return UnityEngine.Input.GetAxis(ControllerInputMapper.GetMapping(input));
        }
    }
}
