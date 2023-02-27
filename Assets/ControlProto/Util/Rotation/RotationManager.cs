using UnityEngine;
using UnityEngine.InputSystem;

namespace ControlProto.Util {
    public class RotationManager {
        private readonly LookInput lookInput;
        private readonly CameraRotator cameraRotator;
        private readonly CharacterRotator characterRotator;

        public RotationManager(Transform character, Transform camera, DefaultInputActions defaultInputActions, float horizontalMouseSensitivity, float verticalMouseSensitivity) {
            lookInput = new LookInput(defaultInputActions);
            cameraRotator = new CameraRotator(camera, verticalMouseSensitivity);
            characterRotator = new CharacterRotator(character, horizontalMouseSensitivity);
        }

        public void PerformRotations() {
            cameraRotator.Rotate(lookInput.Value);
            characterRotator.Rotate(lookInput.Value);
        }
    }
}
