using System;
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
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Transform playerCameraTransform;

        private const float MaxPitch = 90;
        private const float MinPitch = -90;

        private DefaultInputActions defaultInputActions;
        private InputAction crouchAction;
        private InputAction sprintAction;

        private bool isCrouchButtonHeldDown;
        private bool isSprintButtonHeldDown;

        private Vector2 inputMoveVector;
        private Vector2 inputLookVector;

        private float pitch; // up-down rotation around x-axis
        private float yaw; // left-right rotation around y-axis

        private GroundSpeed groundSpeed = GroundSpeed.Walking;
        private PlayerState playerState = PlayerState.Idle;
        private float groundSpeedValue;

        private void Awake() {
            defaultInputActions = new DefaultInputActions();
            defaultInputActions.Player.Move.performed += PlayerMovementCallback;
            defaultInputActions.Player.Move.canceled += PlayerMovementCanceledCallback;

            defaultInputActions.Player.Look.performed += PlayerLookCallback;
            defaultInputActions.Player.Look.canceled += PlayerLookCanceledCallback;
            defaultInputActions.Enable();

            crouchAction = new InputAction("Crouch", InputActionType.Value, "<Keyboard>/leftCtrl");
            crouchAction.started += CrouchStartedCallback;
            crouchAction.canceled += CrouchCanceledCallback;
            crouchAction.Enable();

            sprintAction = new InputAction("Sprint", InputActionType.Value, "<Keyboard>/leftShift");
            sprintAction.started += SprintStartedCallback;
            sprintAction.canceled += SprintCanceledCallback;
            sprintAction.Enable();

            groundSpeedValue = walkMovementSpeed;
        }

        private void Start() {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Offset the character mesh so that it is slightly above the character controller
            playerController.center += new Vector3(0, playerController.skinWidth, 0);
        }

        private void Update() {
            RotatePlayerAndCamera();

            Vector3 moveVector = CalculateHorizontalMovement();

            // if we have horizontal movement, then we might move them off a ledge close to the ground. add a small amount of gravity to pull them down in case.
            // todo: need to make sure this is happening in some way
            if (moveVector != Vector3.zero) {
                moveVector += new Vector3(0, defaultVerticalVelocity, 0);
            }

            if (moveVector != Vector3.zero) {
                MovePlayer(moveVector);
            }

            if (Input.GetKeyDown(KeyCode.Escape)) {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        private void CrouchStartedCallback(InputAction.CallbackContext context) {
            Debug.Log("crouch started");
            isCrouchButtonHeldDown = true;
            UpdateGroundSpeed();
        }

        private void CrouchCanceledCallback(InputAction.CallbackContext context) {
            Debug.Log("crouch canceled");
            isCrouchButtonHeldDown = false;
            UpdateGroundSpeed();
        }

        private void SprintStartedCallback(InputAction.CallbackContext context) {
            Debug.Log("sprint started");
            isSprintButtonHeldDown = true;
            UpdateGroundSpeed();
        }

        private void SprintCanceledCallback(InputAction.CallbackContext context) {
            Debug.Log("sprint canceled");
            isSprintButtonHeldDown = false;
            UpdateGroundSpeed();
        }

        private void OnEnable() {
            defaultInputActions.Enable();
            crouchAction.Enable();
            sprintAction.Enable();
        }

        private void OnDisable() {
            defaultInputActions.Disable();
            crouchAction.Disable();
            sprintAction.Disable();
        }

        private void OnDestroy() {
            defaultInputActions.Dispose();
            crouchAction.Dispose();
            sprintAction.Dispose();
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
            Debug.Log($"Moving player from: {transform.position} to: {moveVector}");
            playerController.Move(moveVector * Time.deltaTime);
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
    }
}
