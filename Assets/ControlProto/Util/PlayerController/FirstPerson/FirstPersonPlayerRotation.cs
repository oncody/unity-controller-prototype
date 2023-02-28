using ControlProto.Util.PlayerInputSystem;
using ControlProto.Util.PlayerRotation;
using UnityEngine;

namespace ControlProto.Util.PlayerController.FirstPerson {
    public class FirstPersonPlayerRotation {
        private readonly IPlayerInputSystem inputSystem;
        private readonly MouseSensitivities mouseSensitivities;

        private float yaw; // left-right rotation around y-axis

        public FirstPersonPlayerRotation(IPlayerInputSystem inputSystem, MouseSensitivities mouseSensitivities) {
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
