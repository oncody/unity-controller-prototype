using System;
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
        [SerializeField] private float ledgeEdgeHorizontalPushForce;

        [SerializeField] private CharacterController player;
        [SerializeField] private Transform playerCamera;

        private const float MaxPitch = 90;
        private const float MinPitch = -90;

        private InputAction jumpAction;
        // private InputAction movementInputAction;

        // Define the input action map
        // private PlayerInputActions playerInputActions;
        // private InputActionMap playerControls;

        // Define the input actions
        // private InputAction moveAction;
        private InputAction rotateAction;

        private Vector2 inputMoveVector;
        // private Vector2 rotationInput;

        // private InputAction movementInputAction;

        private float jumpVelocity;
        private float verticalVelocity;
        private float pitch; // up-down rotation around x-axis
        private float yaw; // left-right rotation around y-axis
        private int groundLayerValue;
        private LayerMask groundLayerMask;
        private float playerTopY;
        private float playerBottomY;
        private float halfPlayerHeight;
        private float rayDistance;
        private bool jumpStarted;
        private bool jumpLeftGround;
        private bool isFallingVertically;
        private bool isGrounded;
        private bool shouldFallBecauseOfGravity;
        private RaycastHit groundedSphereHit;
        private Vector3 previousPosition;

        private void Awake() {
            // InputDevice[] devices = InputSystem.devices.ToArray();
            // foreach (InputDevice device in devices) {
            //     Debug.Log("Device name: " + device.name);
            // }


            // Initialize the input action map
            // playerControls = new InputActionMap();

            // Create the move action and bind it to the WASD keys
            // moveAction = playerControls.AddAction("move");
            // moveAction.AddBinding("<Keyboard>/w");
            // moveAction.AddBinding("<Keyboard>/a");
            // moveAction.AddBinding("<Keyboard>/s");
            // moveAction.AddBinding("<Keyboard>/d");
            var actions = new DefaultInputActions();
            // actions.Player.Look.performed += OnLook;
            actions.Player.Move.performed += PlayerMovementCallback;
            actions.Player.Move.canceled += PlayerMovementCallback;
            actions.Enable();


            // moveAction.started += PlayerMovementCallback;
            // moveAction.started += PlayerMovementCallback;
            // playerInputActions.Gameplay.Move.canceled += OnMoveInput;


            // Create the rotate action and bind it to the mouse delta
            // rotateAction = playerControls.AddAction("rotate");
            // rotateAction.AddBinding("<Mouse>/delta");


            halfPlayerHeight = player.height / 2.0f;
            jumpVelocity = Mathf.Sqrt(2f * jumpHeight * gravity) - defaultVerticalVelocity;
            groundLayerValue = LayerMask.NameToLayer(groundLayer);
            groundLayerMask = 1 << groundLayerValue;
            rayDistance = halfPlayerHeight + groundCheckDistance;

            jumpAction = new InputAction("Jump", InputActionType.Button, "<Keyboard>/space");
            jumpAction.performed += JumpCallback;
            jumpAction.Enable();

            verticalVelocity = defaultVerticalVelocity;

            // Initialize movementInputAction with the Input System asset "PlayerMovement" action map "Move" action
            // movementInputAction = new InputAction("PlayerMovement");
            // movementInputAction.AddBinding("<Keyboard>/w");
            // movementInputAction.AddBinding("<Keyboard>/a");
            // movementInputAction.AddBinding("<Keyboard>/s");
            // movementInputAction.AddBinding("<Keyboard>/d");
            // movementInputAction.started += PlayerMovementCallback;
            // movementInputAction.performed += PlayerMovementCallback;
            // movementInputAction.Enable();

            // playerTopY = playerColliderBounds.center.y + playerColliderBounds.extents.y;
            // playerBottomY = playerColliderBounds.center.y - playerColliderBounds.extents.y;
        }

        private void Start() {
            previousPosition = transform.position;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update() {
            Vector3 currentPosition = transform.position;
            isFallingVertically = Mathf.Abs(currentPosition.y - previousPosition.y) > floatTolerance && currentPosition.y < previousPosition.y;
            PerformGroundCheck();

            Vector3 horizontalMovement = CalculateHorizontalMovement();
            Vector3 verticalMovement = CalculateVerticalMovement();

            // if we have horizontal movement, then we might move them off a ledge close to the ground. add a small amount of gravity to pull them down in case.
            if (horizontalMovement != Vector3.zero && verticalMovement == Vector3.zero) {
                verticalVelocity = defaultVerticalVelocity;
                verticalMovement = new Vector3(0, verticalVelocity, 0);
            }

            Vector3 moveVector = horizontalMovement + verticalMovement;
            if (moveVector != Vector3.zero) {
                MovePlayer(moveVector);
            }

            if (Input.GetKeyDown(KeyCode.Escape)) {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            previousPosition = currentPosition;
        }

        private void LateUpdate() {
            RotatePlayerAndCamera();
        }

        private void JumpCallback(InputAction.CallbackContext context) {
            if (CurrentlyJumping() || isFallingVertically) {
                return;
            }

            PerformGroundCheck();
            if (isGrounded) {
                jumpStarted = true;
                jumpLeftGround = false;
                verticalVelocity += jumpVelocity;
            }
        }

        private void OnEnable() {
            jumpAction.Enable();
            // movementInputAction.Enable();
            // playerControls.Enable();
        }

        private void OnDisable() {
            jumpAction.Disable();
            // playerControls.Disable();
            // movementInputAction.Disable();
        }

        private void RotatePlayerAndCamera() {
            yaw += Input.GetAxisRaw("Mouse X") * horizontalMouseSensitivity;
            pitch -= Input.GetAxisRaw("Mouse Y") * verticalMouseSensitivity;
            pitch = Mathf.Clamp(pitch, MinPitch, MaxPitch);

            // Rotate camera up and down
            playerCamera.localRotation = Quaternion.Euler(pitch, 0, 0);

            // Rotate player left and right
            transform.rotation = Quaternion.Euler(0, yaw, 0);
        }

        private void MovePlayer(Vector3 moveVector) {
            Debug.Log("Moving player");
            player.Move(moveVector * Time.deltaTime);
        }

        private void PerformGroundCheck() {
            isGrounded = isFallingVertically ? false : Physics.SphereCast(transform.position, player.radius, Vector3.down, out groundedSphereHit, rayDistance, groundLayerMask, QueryTriggerInteraction.Ignore);

            if (jumpStarted && !jumpLeftGround && !isGrounded) {
                jumpLeftGround = true;
            }

            if (jumpStarted && jumpLeftGround && isGrounded) {
                jumpStarted = false;
                jumpLeftGround = false;
            }
        }

        private Vector3 CalculateVerticalMovement() {
            shouldFallBecauseOfGravity = false;
            Vector3 moveVector = Vector3.zero;

            if (isGrounded) {
                // ledge push code
                // bool rayCastEncounteredObject = Physics.Raycast(transform.position, Vector3.down, out _, rayDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore);
                //
                // // apply move vector from sphere to point
                // if (!rayCastEncounteredObject) {
                //     // we are near an edge. gently push the person fall off
                //     moveVector = transform.position - groundedSphereHit.point;
                //
                //     // remove vertical from this
                //     moveVector = new Vector3(moveVector.x, 0, moveVector.z);
                //
                //     moveVector = moveVector.magnitude * ledgeEdgeHorizontalPushForce * moveVector.normalized;
                // }
            }

            // verticalVelocity < defaultVerticalVelocity ??
            if (isGrounded && !CurrentlyJumping()) {
                verticalVelocity = defaultVerticalVelocity;
                return moveVector;
            }

            if (!isGrounded) {
                verticalVelocity -= gravity * Time.deltaTime;
                shouldFallBecauseOfGravity = true;
            }

            return moveVector + new Vector3(0, verticalVelocity, 0);
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


        void PlayerMovementCallback(InputAction.CallbackContext context) {
            Debug.Log("movement key presseed!!");
            // Get the input vector from the context
            Debug.Log($"context: {context}");

            inputMoveVector = context.ReadValue<Vector2>();
        }

        private float MovementSpeed() {
            // Input.GetKey checks if it's held down
            if (Input.GetKey(KeyCode.LeftControl)) {
                return crouchMovementSpeed;
            }

            return Input.GetKey(KeyCode.LeftShift) ? sprintMovementSpeed : walkMovementSpeed;
        }

        private bool CurrentlyJumping() {
            return jumpStarted || jumpLeftGround;
        }
    }
}
