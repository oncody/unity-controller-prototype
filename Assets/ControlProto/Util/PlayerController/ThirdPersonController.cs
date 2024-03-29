﻿using Cinemachine;
using ControlProto.Util.Gravity;
using ControlProto.Util.PlayerInputSystem;
using ControlProto.Util.TwoDimensionalGroundPlaneMovement;
using ControlProto.Util.Unity;
using UnityEngine;

namespace ControlProto.Util.PlayerController.ThirdPerson {
    public class ThirdPersonController : PlayerController {
        private readonly IPlayerInputSystem inputSystem;
        private readonly PlayerObjects playerObjects;
        private readonly CinemachineFreeLook virtualCamera;
        private readonly SpeedManager speedManager;
        private readonly PlayerLookConstants lookConstants;

        private float pitch; // up-down rotation around x-axis
        private float yaw; // left-right rotation around y-axis

        public ThirdPersonController(IPlayerInputSystem inputSystem, PlayerObjects playerObjects, GravityManager gravityManager, SpeedManager speedManager, PlayerLookConstants lookConstants) : base(playerObjects, gravityManager) {
            this.inputSystem = inputSystem;
            this.playerObjects = playerObjects;
            this.speedManager = speedManager;
            this.lookConstants = lookConstants;
            virtualCamera = InitializeCamera();
        }

        private CinemachineFreeLook InitializeCamera() {
            GameObject cameraObject = new GameObject("ThirdPersonCamera");
            cameraObject.transform.SetParent(playerObjects.Player);
            // cameraObject.transform.localPosition = new Vector3(0, (playerObjects.Controller.height / 2) - cameraOffsetFromPlayerCeiling, 0);

            CinemachineFreeLook camera = cameraObject.AddComponent<CinemachineFreeLook>();
            camera.m_Follow = playerObjects.Player;
            camera.m_LookAt = playerObjects.Player;
            camera.m_BindingMode = CinemachineTransposer.BindingMode.WorldSpace;
            camera.m_YAxis.m_InvertInput = true;
            camera.m_XAxis.m_InvertInput = false;
            camera.m_XAxis.m_InputAxisName = "";
            camera.m_YAxis.m_InputAxisName = "";

            // top rig
            camera.m_Orbits[0].m_Height = 5;
            camera.m_Orbits[0].m_Radius = 7;

            // middle rig
            camera.m_Orbits[1].m_Height = 3;
            camera.m_Orbits[1].m_Radius = 5;

            // bottom rig
            camera.m_Orbits[2].m_Height = -1;
            camera.m_Orbits[2].m_Radius = 1;

            CinemachineCollider cameraCollider = cameraObject.AddComponent<CinemachineCollider>();
            cameraCollider.m_Strategy = CinemachineCollider.ResolutionStrategy.PullCameraForward;
            cameraCollider.m_CollideAgainst = 1 << LayerMask.NameToLayer("Ground");
            camera.AddExtension(cameraCollider);

            return camera;
        }

        // Right now its automatically bound to mouse input
        // perhaps this needs to be camera and not virtual camera
        public override void PerformRotations() {
            float horizontalInput = inputSystem.HorizontalLookInput();
            float verticalInput = inputSystem.VerticalLookInput();

            yaw += horizontalInput * lookConstants.HorizontalMouseSensitivity;
            pitch -= verticalInput * lookConstants.VerticalMouseSensitivity;
            pitch = Mathf.Clamp(pitch, lookConstants.MinPitch, lookConstants.MaxPitch);

            // virtualCamera.rotation *= Quaternion.Euler(pitch, yaw, 0);
            // virtualCamera.rotation *= Quaternion.Euler(verticalInput, horizontalInput, 0);
            // virtualCamera.m_XAxis.Value = yaw;
            // virtualCamera.m_YAxis.Value = pitch;

            // virtualCamera.m_XAxis.m_InputAxisValue = yaw;
            // virtualCamera.m_YAxis.m_InputAxisValue = pitch;

            virtualCamera.m_XAxis.m_InputAxisValue = horizontalInput * lookConstants.HorizontalMouseSensitivity;
            virtualCamera.m_YAxis.m_InputAxisValue = verticalInput * lookConstants.VerticalMouseSensitivity;
        }

        public override Vector3 TwoDimensionalMovement() {
            // For now this will return a vector 3. Weird but world move direction needs this to be a vector 3 or this will do weird thing. Move the user vertically
            if ((inputSystem.HorizontalMoveInput() == 0) && (inputSystem.VerticalLMoveInput() == 0)) {
                return Vector3.zero;
            }

            float horizontalInput = inputSystem.HorizontalMoveInput();
            float verticalInput = inputSystem.VerticalLMoveInput();
            Vector3 localMoveDirection = new Vector3(horizontalInput, 0, verticalInput);

            // todo: maybe just make sure its not equal to 0?
            if (localMoveDirection.magnitude >= 0.1f) {
                // this needs to get the rotation of the scene camera. not the virtual camera
                float targetangle = Mathf.Atan2(localMoveDirection.x, localMoveDirection.z) * Mathf.Rad2Deg + playerObjects.Camera.eulerAngles.y;
                // float angle = Mathf.SmoothDampAngle(player.eulerAngles.y, targetangle, ref turnVelocity, turnSmoothTime);
                playerObjects.Player.rotation = Quaternion.Euler(0f, targetangle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetangle, 0f) * Vector3.forward;
                return moveDir.normalized * speedManager.Value();
            }

            return Vector3.zero;
        }
    }
}
