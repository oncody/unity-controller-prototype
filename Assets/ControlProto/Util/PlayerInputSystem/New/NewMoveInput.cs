using UnityEngine;
using UnityEngine.InputSystem;

namespace ControlProto.Util.PlayerInputSystem.New {
    public class NewMoveInput : IPlayerVector2Input {
        private Vector2 value = Vector2.zero;

        public NewMoveInput(DefaultInputActions defaultInputActions) {
            defaultInputActions.Player.Move.performed += context => value = context.ReadValue<Vector2>();
            defaultInputActions.Player.Move.canceled += _ => value = Vector2.zero;
        }

        public Vector2 Value() {
            return value;
        }
    }
}
