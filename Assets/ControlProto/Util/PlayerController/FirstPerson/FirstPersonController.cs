using Cinemachine;
using ControlProto.Util.Gravity;
using ControlProto.Util.PlayerInputSystem;
using ControlProto.Util.PlayerRotation;
using ControlProto.Util.TwoDimensionalGroundPlaneMovement;
using UnityEngine;

namespace ControlProto.Util.PlayerController.FirstPerson {
    public class FirstPersonController {
        private readonly FirstPersonThreeDimensionalMovement threeDimensionalMovement;
        private readonly FirstPersonCameraRotation cameraRotation;
        private readonly FirstPersonPlayerRotation playerRotation;

        public FirstPersonController(
            IPlayerInputSystem inputSystem,
            CharacterController controller,
            Transform camera,
            Transform player,
            GravityManager gravityManager,
            SpeedManager speedManager,
            MouseSensitivities mouseSensitivities,
            PitchBounds pitchBounds) {
            cameraRotation = new FirstPersonCameraRotation(inputSystem, camera, mouseSensitivities, pitchBounds);
            playerRotation = new FirstPersonPlayerRotation(inputSystem, player, mouseSensitivities);
            FirstPersonTwoDimensionalMovement firstPersonTwoDimensionalMovement = new FirstPersonTwoDimensionalMovement(inputSystem, speedManager);
            threeDimensionalMovement = new FirstPersonThreeDimensionalMovement(controller, player, firstPersonTwoDimensionalMovement, gravityManager);
        }

        public void Update() {
            cameraRotation.Rotate();
            playerRotation.Rotate();
            threeDimensionalMovement.MovePlayer();
        }
    }
}
