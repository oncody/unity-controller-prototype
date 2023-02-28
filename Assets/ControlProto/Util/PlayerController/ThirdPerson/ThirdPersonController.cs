using ControlProto.Util.Gravity;
using ControlProto.Util.PlayerInputSystem;
using ControlProto.Util.PlayerRotation;
using ControlProto.Util.TwoDimensionalGroundPlaneMovement;
using UnityEngine;

namespace ControlProto.Util.PlayerController.ThirdPerson {
    public class ThirdPersonController : PlayerController {
        private readonly IPlayerMovement playerMovement;
        private readonly ICameraRotation cameraRotation;
        private readonly IPlayerRotation playerRotation;

        public ThirdPersonController(
            IPlayerInputSystem inputSystem,
            CharacterController controller,
            Transform player,
            Transform virtualCamera,
            Transform sceneCamera,
            GravityManager gravityManager,
            SpeedManager speedManager,
            MouseSensitivities mouseSensitivities,
            PitchBounds pitchBounds) {
            cameraRotation = new ThirdPersonCameraRotation(inputSystem, virtualCamera, sceneCamera, mouseSensitivities, pitchBounds);
            playerRotation = new ThirdPersonPlayerRotation(inputSystem, player, virtualCamera, sceneCamera, mouseSensitivities);
            ThirdPersonTwoDimensionalMovement thirdPersonTwoDimensionalMovement = new ThirdPersonTwoDimensionalMovement(inputSystem, player, virtualCamera, sceneCamera, speedManager);
            playerMovement = new ThirdPersonThreeDimensionalMovement(controller, player, thirdPersonTwoDimensionalMovement, gravityManager);
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
