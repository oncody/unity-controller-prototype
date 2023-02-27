using UnityEngine;

namespace ControlProto.Util {
    public class CameraRotator {
        private const float MaxPitch = 90;
        private const float MinPitch = -90;
        private readonly Transform camera;
        private readonly float verticalMouseSensitivity;

        private float pitch; // up-down rotation around x-axis

        public CameraRotator(Transform camera, float verticalMouseSensitivity) {
            this.camera = camera;
            this.verticalMouseSensitivity = verticalMouseSensitivity;
        }

        public void Rotate(Vector2 lookInput) {
            pitch -= lookInput.y * verticalMouseSensitivity;
            pitch = Mathf.Clamp(pitch, MinPitch, MaxPitch);
            camera.localRotation = Quaternion.Euler(pitch, 0, 0);
        }
    }
}
