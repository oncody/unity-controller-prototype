using System;
using System.Collections.Generic;
using ControlProto.Util;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace ControlProto.Scripts.Player {
    public class PlayerMovement : MonoBehaviour {
        [SerializeField] private float verticalMouseSensitivity;
        [SerializeField] private float horizontalMouseSensitivity;
        [SerializeField] private float groundCheckDistance;
        [SerializeField] private float crouchMovementSpeed;
        [SerializeField] private float walkMovementSpeed;
        [SerializeField] private float sprintMovementSpeed;
        [SerializeField] private float gravity;
        [SerializeField] private string groundLayer;
        [SerializeField] private float floatTolerance;
        [SerializeField] private float defaultVerticalVelocity;

        [SerializeField] private CharacterController playerController;
        [SerializeField] private Transform playerCameraTransform;

        private const float MaxPitch = 90;
        private const float MinPitch = -90;

        private readonly List<InputAction> inputActions = new();
        private DefaultInputActions defaultInputActions;

        private GroundSpeed groundSpeed = GroundSpeed.Walking;
        private PlayerState playerState = PlayerState.Idle;

        private Vector3 lastPosition;
        private Vector2 inputMoveVector;
        private Vector2 inputLookVector;

        private float pitch; // up-down rotation around x-axis
        private float yaw; // left-right rotation around y-axis
        private float groundSpeedValue;
        private float verticalVelocity;

        private bool isCrouchButtonHeldDown;
        private bool isSprintButtonHeldDown;
        private bool startedFallingVertically;
        private bool finishedFallingVertically = true;

        private void Awake() {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            verticalVelocity = defaultVerticalVelocity;
            groundSpeedValue = walkMovementSpeed;

            defaultInputActions = new DefaultInputActions();
            defaultInputActions.Player.Move.performed += PlayerMovementCallback;
            defaultInputActions.Player.Move.canceled += PlayerMovementCanceledCallback;
            defaultInputActions.Player.Look.performed += PlayerLookCallback;
            defaultInputActions.Player.Look.canceled += PlayerLookCanceledCallback;

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

        private void Start() {
            // Offset the character mesh so that it is slightly above the character controller
            playerController.center += new Vector3(0, playerController.skinWidth, 0);
        }

        private void Update() {
            UpdateFallingCheck();
            RotatePlayerAndCamera();

            Vector3 moveVector = CalculateHorizontalMovement();
            Vector3 verticalMoveVector = CalculateVerticalMovement();

            // if we have horizontal movement, then we might move them off a ledge close to the ground. add a small amount of gravity to pull them down in case.
            if (moveVector != Vector3.zero && verticalMoveVector == Vector3.zero) {
                verticalMoveVector = new Vector3(0, defaultVerticalVelocity, 0);
            }

            moveVector += verticalMoveVector;
            if (moveVector != Vector3.zero) {
                MovePlayer(moveVector);
            }
        }

        private void ExitFocusCallback(InputAction.CallbackContext context) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void CrouchStartedCallback(InputAction.CallbackContext context) {
            isCrouchButtonHeldDown = true;
            UpdateGroundSpeed();
        }

        private void CrouchCanceledCallback(InputAction.CallbackContext context) {
            isCrouchButtonHeldDown = false;
            UpdateGroundSpeed();
        }

        private void SprintStartedCallback(InputAction.CallbackContext context) {
            isSprintButtonHeldDown = true;
            UpdateGroundSpeed();
        }

        private void SprintCanceledCallback(InputAction.CallbackContext context) {
            isSprintButtonHeldDown = false;
            UpdateGroundSpeed();
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

        private void RotatePlayerAndCamera() {
            yaw += inputLookVector.x * horizontalMouseSensitivity;
            pitch -= inputLookVector.y * verticalMouseSensitivity;

            pitch = Mathf.Clamp(pitch, MinPitch, MaxPitch);

            // Rotate camera up and down
            playerCameraTransform.localRotation = Quaternion.Euler(pitch, 0, 0);

            // Rotate player left and right
            transform.rotation = Quaternion.Euler(0, yaw, 0);
        }

        private void MovePlayer(Vector3 moveVector) {
            // Debug.Log($"Moving player from: {transform.position} to: {moveVector}");
            playerController.Move(moveVector * Time.deltaTime);
        }

        private bool CurrentlyFalling() {
            return startedFallingVertically && !finishedFallingVertically;
        }

        private Vector3 CalculateHorizontalMovement() {
            if (inputMoveVector == Vector2.zero) {
                return Vector3.zero;
            }

            Vector3 inputMoveVector3 = new Vector3(inputMoveVector.x, 0, inputMoveVector.y);
            Vector3 worldMoveDirection = transform.TransformDirection(inputMoveVector3);

            // normalize converts the magnitude to 1 no matter what
            return worldMoveDirection.normalized * groundSpeedValue;
        }

        public Vector3 CalculateVerticalMovement() {
            if (!CurrentlyFalling()) {
                verticalVelocity = defaultVerticalVelocity;
                return Vector3.zero;
            }

            verticalVelocity -= gravity * Time.deltaTime;
            return new Vector3(0, verticalVelocity, 0);
        }

        void PlayerLookCallback(InputAction.CallbackContext context) {
            inputLookVector = context.ReadValue<Vector2>();
        }

        void PlayerLookCanceledCallback(InputAction.CallbackContext context) {
            inputLookVector = Vector2.zero;
        }

        void PlayerMovementCanceledCallback(InputAction.CallbackContext context) {
            inputMoveVector = Vector2.zero;
        }

        void PlayerMovementCallback(InputAction.CallbackContext context) {
            inputMoveVector = context.ReadValue<Vector2>();
        }

        private void UpdateGroundSpeed() {
            if (isCrouchButtonHeldDown) {
                groundSpeed = GroundSpeed.Crouching;
                groundSpeedValue = crouchMovementSpeed;
            }
            else if (isSprintButtonHeldDown) {
                groundSpeed = GroundSpeed.Sprinting;
                groundSpeedValue = sprintMovementSpeed;
            }
            else {
                groundSpeed = GroundSpeed.Walking;
                groundSpeedValue = walkMovementSpeed;
            }
        }

        private void UpdateFallingCheck() {
            float lastYPosition = lastPosition.y;
            float yPosition = transform.position.y;
            bool isFallingVertically = (Mathf.Abs(yPosition - lastYPosition) > floatTolerance) && (yPosition < lastYPosition);

            if (isFallingVertically && !startedFallingVertically) {
                startedFallingVertically = true;
                finishedFallingVertically = false;
            }

            if (!isFallingVertically && startedFallingVertically) {
                startedFallingVertically = false;
                finishedFallingVertically = true;
            }

            lastPosition = transform.position;
        }
    }
}
