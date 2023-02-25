using System;
using ControlProto.Util;
using UnityEngine;
using UnityEngine.InputSystem;

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

        [SerializeField] private CharacterController player;
        [SerializeField] private Transform playerCamera;

        private const float MaxPitch = 90;
        private const float MinPitch = -90;

        private InputAction jumpAction;
        private DefaultInputActions defaultInputActions;

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
        }

        private void Start() {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update() {
            Vector3 currentPosition = transform.position;
            gravityManager.UpdatePlayerPosition(currentPosition);
            RotatePlayerAndCamera();

            Vector3 horizontalMovement = CalculateHorizontalMovement();
            Vector3 verticalMovement = gravityManager.CalculateVerticalMovement(player);

            // if we have horizontal movement, then we might move them off a ledge close to the ground. add a small amount of gravity to pull them down in case.
            // todo: need to make sure this is happening in some way
            if (horizontalMovement != Vector3.zero && verticalMovement == Vector3.zero) {
                verticalMovement = new Vector3(0, defaultVerticalVelocity, 0);
            }

            Vector3 moveVector = horizontalMovement + verticalMovement;
            if (moveVector != Vector3.zero) {
                MovePlayer(moveVector);
            }

            if (Input.GetKeyDown(KeyCode.Escape)) {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        private void JumpCallback(InputAction.CallbackContext context) {
            gravityManager.JumpRequested(player);
        }

        private void OnEnable() {
            jumpAction.Enable();
            defaultInputActions.Enable();
        }

        private void OnDisable() {
            jumpAction.Disable();
            defaultInputActions.Disable();
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

        private void MovePlayer(Vector3 moveVector) {
            Debug.Log($"Moving player: {moveVector}");
            player.Move(moveVector * Time.deltaTime);
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
            // Input.GetKey checks if it's held down
            if (Input.GetKey(KeyCode.LeftControl)) {
                return crouchMovementSpeed;
            }

            return Input.GetKey(KeyCode.LeftShift) ? sprintMovementSpeed : walkMovementSpeed;
        }
    }
}
