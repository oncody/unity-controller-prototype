using ControlProto.Util;
using ControlProto.Util.Input;
using ControlProto.Util.Input.Keyboard;
using UnityEngine;

namespace ControlProto.Scripts.Player {
    public class CursorLock : MonoBehaviour {
        private CursorLocker cursorLocker;
        private KeyboardHandler keyboardHandler;

        void Start() {
            cursorLocker = new CursorLocker();
            keyboardHandler = new KeyboardHandler();

            cursorLocker.LockCursor();
        }

        void Update() {
            if (keyboardHandler.IsActionPressed(KeyboardAction.StopAppFocus)) {
                cursorLocker.LockCursor();
            }
        }
    }
}
