using ControlProto.Util.PlayerInputSystem;
using ControlProto.Util.PlayerRotation;
using UnityEngine;

namespace ControlProto.Util.PlayerController.FirstPerson {
    public class FirstPersonCameraRotation {
        private readonly IPlayerInputSystem inputSystem;
        private readonly MouseSensitivities mouseSensitivities;
        private readonly PitchBounds pitchBounds;

        private float pitch; // up-down rotation around x-axis

        /**
         * This camera needs to be the physical main camera. Not the Virtual Cinemachine camera
         */
        public FirstPersonCameraRotation(IPlayerInputSystem inputSystem, MouseSensitivities mouseSensitivities, PitchBounds pitchBounds) {
            this.inputSystem = inputSystem;
            this.mouseSensitivities = mouseSensitivities;
            this.pitchBounds = pitchBounds;
        }

        public Quaternion Value() {
            float verticalInput = inputSystem.LookInput().Value().y;
            pitch -= verticalInput * mouseSensitivities.Vertical;
            pitch = Mathf.Clamp(pitch, pitchBounds.Min, pitchBounds.Max);
            return Quaternion.Euler(pitch, 0, 0);
        }
    }
}
