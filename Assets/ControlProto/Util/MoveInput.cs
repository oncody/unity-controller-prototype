using UnityEngine;
using UnityEngine.InputSystem;

namespace ControlProto.Util {
    public class MoveInput {
        public Vector2 Value { get; private set; }

        public MoveInput(DefaultInputActions defaultInputActions) {
            defaultInputActions.Player.Move.performed += context => Value = context.ReadValue<Vector2>();
            defaultInputActions.Player.Move.canceled += _ => Value = Vector2.zero;
        }
    }
}
