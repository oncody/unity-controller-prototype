using System;
using System.Collections.Generic;
using Cinemachine;
using ControlProto.Util;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace ControlProto.Scripts.Player {
    public class PlayerMovement : MonoBehaviour {
        [SerializeField] private float verticalMouseSensitivity;
        [SerializeField] private float horizontalMouseSensitivity;
        [SerializeField] private float crouchMovementSpeed;
        [SerializeField] private float walkMovementSpeed;
        [SerializeField] private float sprintMovementSpeed;
        [SerializeField] private float gravity;
        [SerializeField] private float floatTolerance;
        [SerializeField] private float defaultVerticalVelocity;
        [SerializeField] private float cameralOffsetFromPlayerCeiling;

        private CharacterController playerController;
        private Transform playerCamera;
        private LookInput lookInput;
        private CursorManager cursorManager;
        private CharacterControllerMover characterControllerMover;
        private DefaultInputActions defaultInputActions;

        private const float MaxPitch = 90;
        private const float MinPitch = -90;

        private float pitch; // up-down rotation around x-axis
        private float yaw; // left-right rotation around y-axis

        private void Awake() {
            CreateController();
            CreateCamera();
            defaultInputActions = new DefaultInputActions();
            defaultInputActions.Enable();
            cursorManager = new CursorManager();
            characterControllerMover = new CharacterControllerMover(playerController, defaultInputActions, transform, crouchMovementSpeed, walkMovementSpeed, sprintMovementSpeed, gravity, floatTolerance, defaultVerticalVelocity);
            lookInput = new LookInput(defaultInputActions);
        }

        private void Update() {
            characterControllerMover.MovePlayer(transform);
            RotatePlayerAndCamera();
        }

        private void CreateController() {
            // Offset the character mesh so that it is slightly above the character controller. This prevents the player from floating above the ground
            playerController = gameObject.AddComponent<CharacterController>();
            playerController.center += new Vector3(0, playerController.skinWidth, 0);
        }

        private void CreateCamera() {
            GameObject cameraObject = new GameObject("CinemachineVirtualCamera");
            playerCamera = cameraObject.transform;
            playerCamera.SetParent(transform);
            playerCamera.localPosition = new Vector3(0, (playerController.height / 2) - cameralOffsetFromPlayerCeiling, 0);
            CinemachineVirtualCamera cinemachineCameraComponent = cameraObject.AddComponent<CinemachineVirtualCamera>();

            GameObject cameraBrainObject = new GameObject("CameraBrain");
            cameraBrainObject.transform.SetParent(transform);
            Camera cameraComponent = cameraBrainObject.AddComponent<Camera>();
            CinemachineBrain brainComponent = cameraBrainObject.AddComponent<CinemachineBrain>();
        }

        private void RotatePlayerAndCamera() {
            yaw += lookInput.Value.x * horizontalMouseSensitivity;
            pitch -= lookInput.Value.y * verticalMouseSensitivity;
            pitch = Mathf.Clamp(pitch, MinPitch, MaxPitch);

            // Rotate camera up and down
            playerCamera.localRotation = Quaternion.Euler(pitch, 0, 0);

            // Rotate player left and right
            transform.rotation = Quaternion.Euler(0, yaw, 0);
        }

        //
        // private void OnDisable() {
        //     defaultInputActions.Disable();
        //     foreach (InputAction inputAction in inputActions) {
        //         inputAction.Disable();
        //     }
        // }
        //
        // private void OnDestroy() {
        //     defaultInputActions.Dispose();
        //     foreach (InputAction inputAction in inputActions) {
        //         inputAction.Dispose();
        //     }
        // }
    }
}
