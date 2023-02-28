using ControlProto.Util.PlayerInputSystem;
using UnityEngine;

namespace ControlProto.Util.PlayerRotation {
    public class PlayerRotation {
        private readonly IPlayerInputSystem inputSystem;
        private readonly MouseSensitivities mouseSensitivities;

        private float yaw; // left-right rotation around y-axis

        public PlayerRotation(IPlayerInputSystem inputSystem, MouseSensitivities mouseSensitivities) {
            this.inputSystem = inputSystem;
            this.mouseSensitivities = mouseSensitivities;
        }

        public Quaternion Value() {
            float horizontalInput = inputSystem.LookInput().Value().x;
            yaw += horizontalInput * mouseSensitivities.Horizontal;
            return Quaternion.Euler(0, yaw, 0);
        }
    }
}
