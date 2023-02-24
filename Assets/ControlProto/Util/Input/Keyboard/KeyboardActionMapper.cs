using System.Collections.Generic;
using UnityEngine.InputSystem.Controls;

namespace ControlProto.Util.Input.Keyboard {
    public static class KeyboardActionMapper {
        private static readonly Dictionary<KeyboardAction, KeyControl> ActionMap = new() {
            { KeyboardAction.StopAppFocus, UnityEngine.InputSystem.Keyboard.current.escapeKey },
            { KeyboardAction.Jump, UnityEngine.InputSystem.Keyboard.current.spaceKey },
            { KeyboardAction.Crouch, UnityEngine.InputSystem.Keyboard.current.leftCtrlKey },
            { KeyboardAction.Sprint, UnityEngine.InputSystem.Keyboard.current.leftShiftKey },
        };

        public static KeyControl GetMapping(KeyboardAction action) {
            return ActionMap[action];
        }
    }
}
