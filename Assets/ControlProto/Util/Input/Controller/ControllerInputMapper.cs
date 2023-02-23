using System;
using System.Collections.Generic;

namespace ControlProto.Util.Input.Controller {
    public static class ControllerInputMapper {
        private static readonly Dictionary<ControllerInput, String> InputMap = new() {
            { ControllerInput.HorizontalMouseMovement, "Mouse X" },
            { ControllerInput.VerticalMouseMovement, "Mouse Y" },
            { ControllerInput.HorizontalKeyboardMovement, "Horizontal" },
            { ControllerInput.VerticalKeyboardMovement, "Vertical" },
        };

        public static String GetMapping(ControllerInput input) {
            return InputMap[input];
        }
    }
}