using ControlProto.Util.PlayerInputSystem;
using UnityEngine;

namespace ControlProto.Util.PlayerRotation {
    public class PlayerRotation {
        private readonly IPlayerInputSystem inputSystem;
        private readonly Transform player;
        private readonly MouseSensitivities mouseSensitivities;

        private float yaw; // left-right rotation around y-axis

        public PlayerRotation(IPlayerInputSystem inputSystem, Transform player, MouseSensitivities mouseSensitivities) {
            this.inputSystem = inputSystem;
            this.player = player;
            this.mouseSensitivities = mouseSensitivities;
        }

        public void Rotate() {
            float horizontalInput = inputSystem.LookInput().Value().x;
            yaw += horizontalInput * mouseSensitivities.Horizontal;
            player.rotation = Quaternion.Euler(0, yaw, 0);
        }
    }
}
