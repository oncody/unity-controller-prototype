using System;
using System.Collections.Generic;

namespace ControlProto.Util.Input.Controller {
    public static class ControllerInputMapper {
        private static readonly Dictionary<ControllerInput, String> InputMap = new() {
        };

        public static String GetMapping(ControllerInput input) {
            return InputMap[input];
        }
    }
}
