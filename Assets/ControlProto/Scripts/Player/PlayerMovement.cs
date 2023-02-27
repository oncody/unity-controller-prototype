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

        private CursorManager cursorManager;
        private CharacterControllerMover characterControllerMover;
        private DefaultInputActions defaultInputActions;
        private RotationManager rotationManager;

        private void Awake() {
            CharacterController controller = CreateController();
            Transform camera = CreateCamera(controller);
            defaultInputActions = new DefaultInputActions();
            defaultInputActions.Enable();
            cursorManager = new CursorManager();
            characterControllerMover = new CharacterControllerMover(controller, defaultInputActions, transform, crouchMovementSpeed, walkMovementSpeed, sprintMovementSpeed, gravity, floatTolerance, defaultVerticalVelocity);
            rotationManager = new RotationManager(transform, camera, defaultInputActions, horizontalMouseSensitivity, verticalMouseSensitivity);
        }

        private void Update() {
            characterControllerMover.MovePlayer(transform);
            rotationManager.PerformRotations();
        }

        private CharacterController CreateController() {
            // Offset the character mesh so that it is slightly above the character controller. This prevents the player from floating above the ground
            CharacterController controller = gameObject.AddComponent<CharacterController>();
            controller.center += new Vector3(0, controller.skinWidth, 0);
            return controller;
        }

        private Transform CreateCamera(CharacterController controller) {
            GameObject cameraObject = new GameObject("CinemachineVirtualCamera");
            Transform camera = cameraObject.transform;
            camera.SetParent(transform);
            camera.localPosition = new Vector3(0, (controller.height / 2) - cameralOffsetFromPlayerCeiling, 0);
            CinemachineVirtualCamera cinemachineCameraComponent = cameraObject.AddComponent<CinemachineVirtualCamera>();

            GameObject cameraBrainObject = new GameObject("CameraBrain");
            cameraBrainObject.transform.SetParent(transform);
            Camera cameraComponent = cameraBrainObject.AddComponent<Camera>();
            CinemachineBrain brainComponent = cameraBrainObject.AddComponent<CinemachineBrain>();
            return camera;
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
