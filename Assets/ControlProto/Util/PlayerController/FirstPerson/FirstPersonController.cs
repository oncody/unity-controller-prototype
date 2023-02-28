using Cinemachine;
using ControlProto.Util.Gravity;
using ControlProto.Util.PlayerInputSystem;
using ControlProto.Util.PlayerRotation;
using ControlProto.Util.TwoDimensionalGroundPlaneMovement;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace ControlProto.Util.PlayerController.FirstPerson {
    public class FirstPersonController {
        private readonly CharacterController controller;
        private readonly CinemachineVirtualCamera virtualCamera;
        private readonly Transform player;
        private readonly FirstPersonThreeDimensionalMovement threeDimensionalMovement;
        private readonly FirstPersonCameraRotation cameraRotation;
        private readonly FirstPersonPlayerRotation playerRotation;

        public FirstPersonController(
            IPlayerInputSystem inputSystem,
            CharacterController controller,
            CinemachineVirtualCamera virtualCamera,
            Transform player,
            GravityManager gravityManager,
            SpeedManager speedManager,
            MouseSensitivities mouseSensitivities,
            PitchBounds pitchBounds) {
            this.controller = controller;
            this.virtualCamera = virtualCamera;
            this.player = player;
            cameraRotation = new FirstPersonCameraRotation(inputSystem, mouseSensitivities, pitchBounds);
            playerRotation = new FirstPersonPlayerRotation(inputSystem, mouseSensitivities);
            FirstPersonTwoDimensionalMovement firstPersonTwoDimensionalMovement = new FirstPersonTwoDimensionalMovement(inputSystem, speedManager);
            threeDimensionalMovement = new FirstPersonThreeDimensionalMovement(firstPersonTwoDimensionalMovement, gravityManager);
        }

        public void Update() {
            RotateCamera();
            RotatePlayer();
            MovePlayer();
        }

        private void RotateCamera() {
            virtualCamera.transform.localRotation = cameraRotation.Value();
        }

        private void RotatePlayer() {
            player.rotation = playerRotation.Value();
        }

        private void MovePlayer() {
            Vector3 movementVector = threeDimensionalMovement.Value(player);
            if (movementVector != Vector3.zero) {
                // Debug.Log($"Moving player from: {transform.position} to: {moveVector}");
                controller.Move(movementVector * Time.deltaTime);
            }
        }
    }
}
