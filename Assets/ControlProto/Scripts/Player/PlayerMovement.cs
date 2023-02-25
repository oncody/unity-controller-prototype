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
        [SerializeField] private float defaultVerticalVelocity;
        [SerializeField] private float jumpHeight;
        [SerializeField] private string groundLayer;
        [SerializeField] private CharacterController player;
        [SerializeField] private Transform playerCamera;

        private const float MaxPitch = 90;
        private const float MinPitch = -90;

        private InputAction jumpAction;
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

        private void Awake() {
            halfPlayerHeight = player.height / 2.0f;
            jumpVelocity = Mathf.Sqrt(2f * jumpHeight * gravity);
            groundLayerValue = LayerMask.NameToLayer(groundLayer);
            groundLayerMask = 1 << groundLayerValue;
            rayDistance = halfPlayerHeight + groundCheckDistance;

            // Initialize the Jump action
            jumpAction = new InputAction("Jump", InputActionType.Button, null, null);
            jumpAction.AddBinding("<Keyboard>/space");

            // Register a callback function for the Jump action
            jumpAction.performed += Jump;

            // playerTopY = playerColliderBounds.center.y + playerColliderBounds.extents.y;
            // playerBottomY = playerColliderBounds.center.y - playerColliderBounds.extents.y;
        }

        private void Jump(InputAction.CallbackContext context) {
            Debug.Log("Jump!");
            if (jumpStarted || jumpLeftGround || !PerformGroundCheck()) {
                return;
            }

            jumpStarted = true;
            verticalVelocity += jumpVelocity;
        }

        private void OnEnable() {
            jumpAction.Enable();
        }

        private void OnDisable() {
            jumpAction.Disable();
        }

        private void Start() {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update() {
            Vector3 horizontalMovement = CalculateHorizontalMovement();
            Vector3 verticalMovement = CalculateVerticalMovement();
            MovePlayer(horizontalMovement + verticalMovement);

            if (Input.GetKeyDown(KeyCode.Escape)) {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        private void LateUpdate() {
            RotatePlayerAndCamera();
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
            if (moveVector != Vector3.zero) {
                player.Move(moveVector * Time.deltaTime);
            }
        }

        private bool PerformGroundCheck() {
            bool isGrounded = Physics.SphereCast(transform.position, player.radius, Vector3.down, out _, rayDistance, groundLayerMask, QueryTriggerInteraction.Ignore);
            if (jumpStarted && !jumpLeftGround && !isGrounded) {
                jumpLeftGround = true;
            }

            if (jumpStarted && jumpLeftGround && isGrounded) {
                jumpStarted = false;
                jumpLeftGround = false;
            }

            Debug.Log($"Grounded: {isGrounded}");
            return isGrounded;
        }

        private Vector3 CalculateVerticalMovement() {
            bool isGrounded = PerformGroundCheck();
            if (!isGrounded) {
                verticalVelocity -= gravity * Time.deltaTime;
            }

            bool isFalling = verticalVelocity < defaultVerticalVelocity;
            if (isGrounded && isFalling) {
                verticalVelocity = defaultVerticalVelocity;
            }

            return new Vector3(0, verticalVelocity, 0);
        }

        private Vector3 CalculateHorizontalMovement() {
            Vector3 keyboardMovementVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            if (keyboardMovementVector == Vector3.zero) {
                return Vector3.zero;
            }

            Vector3 worldMoveDirection = transform.TransformDirection(keyboardMovementVector);
            // normalize converts the magnitude to 1 no matter what
            return worldMoveDirection.normalized * MovementSpeed();
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
