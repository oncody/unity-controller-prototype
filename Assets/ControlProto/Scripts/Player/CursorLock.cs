using ControlProto.Util;
using ControlProto.Util.Input.Keyboard;
using UnityEngine;

namespace ControlProto.Scripts.Player {
    public class CursorLock : MonoBehaviour {
        private readonly CursorLocker cursorLocker = new();
        private readonly KeyboardHandler keyboardHandler = new();

        private void Start() {
            cursorLocker.LockCursor();
        }

        private void Update() {
            if (keyboardHandler.IsActionPressed(KeyboardAction.StopAppFocus)) {
                cursorLocker.LockCursor();
            }
        }
    }
}
