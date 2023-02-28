using ControlProto.Util.PlayerInputSystem;
using UnityEngine;

namespace ControlProto.Util.PlayerRotation {
    public class CharacterRotator {
        private readonly IPlayerInputSystem inputSystem;
        private readonly Transform character;
        private readonly MouseSensitivities mouseSensitivities;

        private float yaw; // left-right rotation around y-axis

        public CharacterRotator(IPlayerInputSystem inputSystem, Transform character, MouseSensitivities mouseSensitivities) {
            this.inputSystem = inputSystem;
            this.character = character;
            this.mouseSensitivities = mouseSensitivities;
        }

        public void Rotate() {
            float horizontalInput = inputSystem.LookInput().Value().x;
            yaw += horizontalInput * mouseSensitivities.Horizontal;
            character.rotation = Quaternion.Euler(0, yaw, 0);
        }
    }
}
