using Cinemachine;
using ControlProto.Util.Gravity;
using ControlProto.Util.PlayerInputSystem;
using ControlProto.Util.TwoDimensionalGroundPlaneMovement;
using ControlProto.Util.Unity;
using UnityEngine;

namespace ControlProto.Util.PlayerController.FirstPerson {
    public class FirstPersonController : PlayerController {
        private readonly IPlayerInputSystem inputSystem;
        private readonly PlayerObjects playerObjects;
        private readonly Transform virtualCamera;
        private readonly SpeedManager speedManager;
        private readonly PlayerLookConstants lookConstants;
        private readonly float cameraOffsetFromPlayerCeiling;

        private float pitch; // up-down rotation around x-axis
        private float yaw; // left-right rotation around y-axis

        public FirstPersonController(IPlayerInputSystem inputSystem, PlayerObjects playerObjects, GravityManager gravityManager, SpeedManager speedManager, PlayerLookConstants lookConstants, float cameraOffsetFromPlayerCeiling) : base(playerObjects, gravityManager) {
            this.inputSystem = inputSystem;
            this.playerObjects = playerObjects;
            this.speedManager = speedManager;
            this.lookConstants = lookConstants;
            this.cameraOffsetFromPlayerCeiling = cameraOffsetFromPlayerCeiling;
            virtualCamera = InitializeCamera();
        }

        private Transform InitializeCamera() {
            GameObject cameraObject = new GameObject("CinemachineVirtualCamera");
            cameraObject.transform.SetParent(playerObjects.Player);
            cameraObject.transform.localPosition = new Vector3(0, (playerObjects.Controller.height / 2) - cameraOffsetFromPlayerCeiling, 0);
            CinemachineVirtualCamera camera = cameraObject.AddComponent<CinemachineVirtualCamera>();
            return camera.transform;
        }

        // todo: try to change this to the virtual camera
        public override void RotateCamera() {
            float verticalInput = inputSystem.VerticalLookInput();
            pitch -= verticalInput * lookConstants.VerticalMouseSensitivity;
            pitch = Mathf.Clamp(pitch, lookConstants.MinPitch, lookConstants.MaxPitch);
            virtualCamera.localRotation = Quaternion.Euler(pitch, 0, 0);
        }

        public override void RotatePlayer() {
            float horizontalInput = inputSystem.HorizontalLookInput();
            yaw += horizontalInput * lookConstants.HorizontalMouseSensitivity;
            playerObjects.Player.rotation = Quaternion.Euler(0, yaw, 0);
        }

        public override Vector3 TwoDimensionalMovement() {
            // For now this will return a vector 3. Weird but world move direction needs this to be a vector 3 or this will do weird thing. Move the user vertically
            if ((inputSystem.HorizontalMoveInput() == 0) && (inputSystem.VerticalLMoveInput() == 0)) {
                return Vector3.zero;
            }

            float horizontalInput = inputSystem.HorizontalMoveInput();
            float verticalInput = inputSystem.VerticalLMoveInput();
            Vector3 localMoveDirection = new Vector3(horizontalInput, 0, verticalInput);
            Vector3 worldMoveDirection = playerObjects.Player.TransformDirection(localMoveDirection);
            return worldMoveDirection.normalized * speedManager.Value();
        }
    }
}
