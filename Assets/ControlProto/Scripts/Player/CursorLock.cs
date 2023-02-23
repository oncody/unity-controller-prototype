using ControlProto.Util;
using ControlProto.Util.Input.Keyboard;
using UnityEngine;

namespace ControlProto.Scripts.Player {
    public class CursorLock : MonoBehaviour {
        private readonly CursorLocker cursorLocker = new();
        private readonly KeyboardHandler keyboardHandler = new();

        void Start() {
            cursorLocker.LockCursor();
        }

        void Update() {
            if (keyboardHandler.IsActionPressed(KeyboardAction.StopAppFocus)) {
                cursorLocker.LockCursor();
            }
        }
    }
}
