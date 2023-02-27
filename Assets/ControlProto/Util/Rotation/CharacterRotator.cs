using UnityEngine;
using UnityEngine.UIElements;

namespace ControlProto.Util {
    public class CharacterRotator {
        private readonly Transform character;
        private readonly float horizontalMouseSensitivity;

        private float yaw; // left-right rotation around y-axis

        public CharacterRotator(Transform character, float horizontalMouseSensitivity) {
            this.character = character;
            this.horizontalMouseSensitivity = horizontalMouseSensitivity;
        }

        public void Rotate(Vector2 lookInput) {
            yaw += lookInput.x * horizontalMouseSensitivity;
            character.rotation = Quaternion.Euler(0, yaw, 0);
        }
    }
}
