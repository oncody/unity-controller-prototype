using Cinemachine;
using ControlProto.Util.Gravity;
using ControlProto.Util.PlayerInputSystem;
using ControlProto.Util.PlayerRotation;
using ControlProto.Util.TwoDimensionalGroundPlaneMovement;
using UnityEngine;

namespace ControlProto.Util.PlayerController.FirstPerson {
    public class FirstPersonController : PlayerController {
        private readonly IPlayerMovement playerMovement;
        private readonly ICameraRotation cameraRotation;
        private readonly IPlayerRotation playerRotation;

        public FirstPersonController(
            IPlayerInputSystem inputSystem,
            CharacterController controller,
            Transform player,
            Transform camera,
            GravityManager gravityManager,
            SpeedManager speedManager,
            MouseSensitivities mouseSensitivities,
            PitchBounds pitchBounds) {
            cameraRotation = new FirstPersonCameraRotation(inputSystem, camera, mouseSensitivities, pitchBounds);
            playerRotation = new FirstPersonPlayerRotation(inputSystem, player, mouseSensitivities);
            FirstPersonTwoDimensionalMovement firstPersonTwoDimensionalMovement = new FirstPersonTwoDimensionalMovement(inputSystem, speedManager);
            playerMovement = new FirstPersonThreeDimensionalMovement(controller, player, firstPersonTwoDimensionalMovement, gravityManager);
        }

        public override ICameraRotation CameraRotation() {
            return cameraRotation;
        }

        public override IPlayerMovement PlayerMovement() {
            return playerMovement;
        }

        public override IPlayerRotation PlayerRotation() {
            return playerRotation;
        }
    }
}
