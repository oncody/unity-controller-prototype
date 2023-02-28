using ControlProto.Util.PlayerInputSystem;
using ControlProto.Util.PlayerRotation;
using UnityEngine;

namespace ControlProto.Util.PlayerController.ThirdPerson {
    public class ThirdPersonPlayerRotation : IPlayerRotation {
        private readonly IPlayerInputSystem inputSystem;
        private readonly Transform player;
        private readonly Transform virtualCamera;
        private readonly Transform sceneCamera;
        private readonly MouseSensitivities mouseSensitivities;

        private float yaw; // left-right rotation around y-axis

        public ThirdPersonPlayerRotation(IPlayerInputSystem inputSystem, Transform player, Transform virtualCamera, Transform sceneCamera, MouseSensitivities mouseSensitivities) {
            this.inputSystem = inputSystem;
            this.player = player;
            this.virtualCamera = virtualCamera;
            this.sceneCamera = sceneCamera;
            this.mouseSensitivities = mouseSensitivities;
        }

        public void Rotate() {
            float horizontalInput = inputSystem.LookInput().Value().x;
            yaw += horizontalInput * mouseSensitivities.Horizontal;
            // player.rotation = Quaternion.Euler(0, yaw, 0);
        }
    }
}
