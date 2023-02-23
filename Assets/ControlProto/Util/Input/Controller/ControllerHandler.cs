namespace ControlProto.Util.Input.Controller {
    public class ControllerHandler {
        public float HorizontalMouseInput() {
            return SmoothValue(ControllerInput.HorizontalMouseInput);
        }

        public float VerticalMouseInput() {
            return SmoothValue(ControllerInput.VerticalMouseInput);
        }

        public float HorizontalMovementInput() {
            return SmoothValue(ControllerInput.HorizontalMovement);
        }

        public float VerticalMovementInput() {
            return SmoothValue(ControllerInput.VerticalMovement);
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
