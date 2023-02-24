using System.Collections.Generic;
using UnityEngine;

namespace ControlProto.Util.Input.Keyboard {
    public static class KeyboardActionMapper {
        private static readonly Dictionary<KeyboardAction, KeyCode> ActionMap = new() {
            { KeyboardAction.StopAppFocus, KeyCode.Escape },
            { KeyboardAction.Jump, KeyCode.Space },
        };

        public static KeyCode GetMapping(KeyboardAction action) {
            return ActionMap[action];
        }
    }
}
