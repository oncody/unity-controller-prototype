﻿using ControlProto.Util.PlayerInputSystem;
using UnityEngine;

namespace ControlProto.Util.PlayerRotation {
    public class CameraRotation {
        private readonly IPlayerInputSystem inputSystem;
        private readonly Transform camera;
        private readonly MouseSensitivities mouseSensitivities;
        private readonly PitchBounds pitchBounds;

        private float pitch; // up-down rotation around x-axis

        /**
         * This camera needs to be the physical main camera. Not the Virtual Cinemachine camera
         */
        public CameraRotation(IPlayerInputSystem inputSystem, Transform camera, MouseSensitivities mouseSensitivities, PitchBounds pitchBounds) {
            this.inputSystem = inputSystem;
            this.camera = camera;
            this.mouseSensitivities = mouseSensitivities;
            this.pitchBounds = pitchBounds;
        }

        public void Rotate() {
            float verticalInput = inputSystem.LookInput().Value().y;
            pitch -= verticalInput * mouseSensitivities.Vertical;
            pitch = Mathf.Clamp(pitch, pitchBounds.Min, pitchBounds.Max);
            camera.localRotation = Quaternion.Euler(pitch, 0, 0);
        }
    }
}