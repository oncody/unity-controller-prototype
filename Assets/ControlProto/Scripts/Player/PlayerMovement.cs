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
        [SerializeField] private float jumpHeight;
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
        private InputAction jumpAction;

        private bool isCrouchButtonHeldDown;
        private bool isSprintButtonHeldDown;

        private Vector2 inputMoveVector;
        private Vector2 inputLookVector;

        private GravityManager gravityManager;
        private float pitch; // up-down rotation around x-axis
        private float yaw; // left-right rotation around y-axis

        private float playerTopY;
        private float playerBottomY;

        private void Awake() {
            gravityManager = new GravityManager(groundLayer, defaultVerticalVelocity, jumpHeight, gravity, groundCheckDistance, floatTolerance);

            defaultInputActions = new DefaultInputActions();
            defaultInputActions.Player.Move.performed += PlayerMovementCallback;
            defaultInputActions.Player.Move.canceled += PlayerMovementCanceledCallback;

            defaultInputActions.Player.Look.performed += PlayerLookCallback;
            defaultInputActions.Player.Look.canceled += PlayerLookCanceledCallback;
            defaultInputActions.Enable();

            jumpAction = new InputAction("Jump", InputActionType.Button, "<Keyboard>/space");
            jumpAction.performed += JumpCallback;
            jumpAction.Enable();

            crouchAction = new InputAction("Crouch", InputActionType.Value, "<Keyboard>/leftCtrl");
            crouchAction.started += CrouchStartedCallback;
            crouchAction.canceled += CrouchCanceledCallback;
            crouchAction.Enable();

            sprintAction = new InputAction("Sprint", InputActionType.Value, "<Keyboard>/leftShift");
            sprintAction.started += SprintStartedCallback;
            sprintAction.canceled += SprintCanceledCallback;
            sprintAction.Enable();
        }

        private void Start() {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Offset the character mesh so that it is slightly above the character controller
            playerController.center += new Vector3(0, playerController.skinWidth, 0);
            // playerTransform.position -= new Vector3(0, playerController.skinWidth, 0);
        }

        private void Update() {
            Vector3 currentPosition = transform.position;
            gravityManager.UpdatePlayerPosition(currentPosition);
            RotatePlayerAndCamera();

            Vector3 horizontalMovement = CalculateHorizontalMovement();
            Vector3 verticalMovement = gravityManager.CalculateVerticalMovement(playerController, transform);

            // if we have horizontal movement, then we might move them off a ledge close to the ground. add a small amount of gravity to pull them down in case.
            // todo: need to make sure this is happening in some way
            // if (horizontalMovement != Vector3.zero && verticalMovement == Vector3.zero) {
            // verticalMovement = new Vector3(0, defaultVerticalVelocity, 0);
            // }

            Vector3 moveVector = horizontalMovement + verticalMovement;
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
        }

        private void CrouchCanceledCallback(InputAction.CallbackContext context) {
            Debug.Log("crouch canceled");
            isCrouchButtonHeldDown = false;
        }

        private void SprintStartedCallback(InputAction.CallbackContext context) {
            Debug.Log("sprint started");
            isSprintButtonHeldDown = true;
        }

        private void SprintCanceledCallback(InputAction.CallbackContext context) {
            Debug.Log("sprint canceled");
            isSprintButtonHeldDown = false;
        }

        private void JumpCallback(InputAction.CallbackContext context) {
            gravityManager.JumpRequested(playerController, transform);
        }

        private void OnEnable() {
            jumpAction.Enable();
            defaultInputActions.Enable();
            crouchAction.Enable();
            sprintAction.Enable();
        }

        private void OnDisable() {
            jumpAction.Disable();
            defaultInputActions.Disable();
            crouchAction.Disable();
            sprintAction.Disable();
        }

        private void OnDestroy() {
            jumpAction.Dispose();
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
            return worldMoveDirection.normalized * MovementSpeed();
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

        private float MovementSpeed() {
            if (isCrouchButtonHeldDown) {
                return crouchMovementSpeed;
            }

            return isSprintButtonHeldDown ? sprintMovementSpeed : walkMovementSpeed;
        }
    }
}
