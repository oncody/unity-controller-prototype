using UnityEngine;
using UnityEngine.InputSystem;

namespace ControlProto.Util.PlayerInputSystem.New {
    public class NewLookInput : IPlayerVector2Input {
        private Vector2 value = Vector2.zero;

        public NewLookInput(DefaultInputActions defaultInputActions) {
            defaultInputActions.Player.Look.performed += context => value = context.ReadValue<Vector2>();
            defaultInputActions.Player.Look.canceled += _ => value = Vector2.zero;
        }

        public Vector2 Value() {
            return value;
        }
    }
}