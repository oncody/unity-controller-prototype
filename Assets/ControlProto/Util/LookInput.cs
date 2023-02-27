using UnityEngine;
using UnityEngine.InputSystem;

namespace ControlProto.Util {
    public class LookInput {
        public Vector2 Value { get; private set; }

        public LookInput(DefaultInputActions defaultInputActions) {
            defaultInputActions.Player.Look.performed += context => Value = context.ReadValue<Vector2>();
            defaultInputActions.Player.Look.canceled += _ => Value = Vector2.zero;
        }
    }
}
