using System;
using System.Collections.Generic;

namespace ControlProto.Util.Input.Controller {
    public static class ControllerInputMapper {
        private static readonly Dictionary<ControllerInput, String> InputMap = new() {
            { ControllerInput.HorizontalMouseInput, "Mouse X" },
            { ControllerInput.VerticalMouseInput, "Mouse Y" },
            { ControllerInput.HorizontalMovement, "Horizontal" },
            { ControllerInput.VerticalMovement, "Vertical" },
        };

        public static String GetMapping(ControllerInput input) {
            return InputMap[input];
        }
    }
}
