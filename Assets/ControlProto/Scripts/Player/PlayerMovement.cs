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

        private GravityManager gravityManager;
        private CharacterController playerController;
        private Transform playerCamera;

        private const float MaxPitch = 90;
        private const float MinPitch = -90;

        private readonly List<InputAction> inputActions = new();
        private DefaultInputActions defaultInputActions;

        private GroundSpeed groundSpeed = GroundSpeed.Walking;
        private PlayerState playerState = PlayerState.Idle;

        private Vector2 inputMoveVector;
        private Vector2 inputLookVector;

        private float pitch; // up-down rotation around x-axis
        private float yaw; // left-right rotation around y-axis
        private float groundSpeedValue;

        private bool isCrouchButtonHeldDown;
        private bool isSprintButtonHeldDown;

        private void Awake() {
            groundSpeedValue = walkMovementSpeed;
            gravityManager = new GravityManager(gravity, floatTolerance, defaultVerticalVelocity, transform.position.y);
            LockCursor();
            BindInput();
        }

        private void Start() {
            CreateController();
            CreateCamera();
        }

        private void Update() {
            gravityManager.UpdateFallingCheck(transform.position.y);
            RotatePlayerAndCamera();
            MovePlayer();
        }

        private void ExitFocusCallback(InputAction.CallbackContext context) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void CrouchStartedCallback(InputAction.CallbackContext context) {
            isCrouchButtonHeldDown = true;
            groundSpeed = GroundSpeed.Crouching;
            groundSpeedValue = crouchMovementSpeed;
        }

        private void CrouchCanceledCallback(InputAction.CallbackContext context) {
            isCrouchButtonHeldDown = false;
            groundSpeed = isSprintButtonHeldDown ? GroundSpeed.Sprinting : GroundSpeed.Walking;
            groundSpeedValue = isSprintButtonHeldDown ? sprintMovementSpeed : walkMovementSpeed;
        }

        private void SprintStartedCallback(InputAction.CallbackContext context) {
            isSprintButtonHeldDown = true;
            groundSpeed = isCrouchButtonHeldDown ? GroundSpeed.Crouching : GroundSpeed.Sprinting;
            groundSpeedValue = isCrouchButtonHeldDown ? crouchMovementSpeed : sprintMovementSpeed;
        }

        private void SprintCanceledCallback(InputAction.CallbackContext context) {
            isSprintButtonHeldDown = false;
            groundSpeed = isCrouchButtonHeldDown ? GroundSpeed.Crouching : GroundSpeed.Walking;
            groundSpeedValue = isCrouchButtonHeldDown ? crouchMovementSpeed : walkMovementSpeed;
        }

        private void OnEnable() {
            defaultInputActions.Enable();
            foreach (InputAction inputAction in inputActions) {
                inputAction.Enable();
            }
        }

        private void OnDisable() {
            defaultInputActions.Disable();
            foreach (InputAction inputAction in inputActions) {
                inputAction.Disable();
            }
        }

        private void OnDestroy() {
            defaultInputActions.Dispose();
            foreach (InputAction inputAction in inputActions) {
                inputAction.Dispose();
            }
        }

        private void LockCursor() {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void CreateController() {
            // Offset the character mesh so that it is slightly above the character controller. This prevents the player from floating
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
            yaw += inputLookVector.x * horizontalMouseSensitivity;
            pitch -= inputLookVector.y * verticalMouseSensitivity;
            pitch = Mathf.Clamp(pitch, MinPitch, MaxPitch);

            // Rotate camera up and down
            playerCamera.localRotation = Quaternion.Euler(pitch, 0, 0);

            // Rotate player left and right
            transform.rotation = Quaternion.Euler(0, yaw, 0);
        }

        private void MovePlayer() {
            Vector3 movementVector = CalculateTwoDimensionalMovement();
            float verticalMoveValue = gravityManager.CalculateVerticalMovement();

            // if we have horizontal movement, then we might move them off a ledge close to the ground. add a small amount of gravity to pull them down in case.
            if (movementVector != Vector3.zero && verticalMoveValue == 0) {
                verticalMoveValue = defaultVerticalVelocity;
            }

            movementVector += new Vector3(0, verticalMoveValue, 0);
            if (movementVector != Vector3.zero) {
                // Debug.Log($"Moving player from: {transform.position} to: {moveVector}");
                playerController.Move(movementVector * Time.deltaTime);
            }
        }

        private Vector3 CalculateTwoDimensionalMovement() {
            if (inputMoveVector == Vector2.zero) {
                return Vector3.zero;
            }

            Vector3 localMoveDirection = new Vector3(inputMoveVector.x, 0, inputMoveVector.y);
            Vector3 worldMoveDirection = transform.TransformDirection(localMoveDirection);
            return worldMoveDirection.normalized * groundSpeedValue;
        }

        private void BindInput() {
            defaultInputActions = new DefaultInputActions();
            defaultInputActions.Player.Move.performed += context => inputMoveVector = context.ReadValue<Vector2>();
            defaultInputActions.Player.Move.canceled += _ => inputMoveVector = Vector2.zero;

            defaultInputActions.Player.Look.performed += context => inputLookVector = context.ReadValue<Vector2>();
            defaultInputActions.Player.Look.canceled += _ => inputLookVector = Vector2.zero;

            InputAction crouchAction = new InputAction("Crouch", InputActionType.Value, "<Keyboard>/leftCtrl");
            crouchAction.started += CrouchStartedCallback;
            crouchAction.canceled += CrouchCanceledCallback;
            inputActions.Add(crouchAction);

            InputAction sprintAction = new InputAction("Sprint", InputActionType.Value, "<Keyboard>/leftShift");
            sprintAction.started += SprintStartedCallback;
            sprintAction.canceled += SprintCanceledCallback;
            inputActions.Add(sprintAction);

            InputAction exitFocusAction = new InputAction("ExitFocus", InputActionType.Button, "<Keyboard>/escape");
            exitFocusAction.performed += ExitFocusCallback;
            inputActions.Add(exitFocusAction);

            defaultInputActions.Enable();
            foreach (InputAction inputAction in inputActions) {
                inputAction.Enable();
            }
        }
    }
}
