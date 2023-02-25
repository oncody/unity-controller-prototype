using UnityEngine;

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
        [SerializeField] private CharacterController player;
        [SerializeField] private Transform playerCamera;
        [SerializeField] private string groundLayer;

        private const float MaxPitch = 90;
        private const float MinPitch = -90;

        private float jumpVelocity;
        private float verticalVelocity;
        private float pitch; // up-down rotation around x-axis
        private float yaw; // left-right rotation around y-axis
        private int groundLayerValue;
        private LayerMask groundLayerMask;
        private bool isGrounded = false;
        private Collider playerCollider;
        private Bounds playerColliderBounds;
        private float playerTopY;
        private float playerBottomY;

        private void Start() {
            jumpVelocity = Mathf.Sqrt(2f * jumpHeight * gravity);
            groundLayerValue = LayerMask.NameToLayer(groundLayer);
            groundLayerMask = 1 << groundLayerValue;
            playerCollider = GetComponent<Collider>();
            playerCollider.isTrigger = true;

            playerColliderBounds = playerCollider.bounds;
            playerTopY = playerColliderBounds.center.y + playerColliderBounds.extents.y;
            playerBottomY = playerColliderBounds.center.y - playerColliderBounds.extents.y;

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

        private Vector3 CalculateVerticalMovement() {
            Vector3 verticalMoveVector = Vector3.zero;

            // Start the raycast at the center point of our character along x, y, and z
            // Set the max ray distance to be slightly larger than half of the character's height
            float rayDistance = (player.height / 2.0f) + groundCheckDistance;

            // Debug.Log($"Ray origin: {transform.position}");
            // Debug.Log($"Ray distance: {rayDistance}");
            isGrounded = Physics.SphereCast(transform.position, player.radius, Vector3.down, out _, rayDistance, groundLayerMask, QueryTriggerInteraction.Ignore);
            // Debug.Log($"Grounded: {grounded}");

            // Input.GetKeyDown checks if a key is pressed down
            if (isGrounded && Input.GetKeyDown(KeyCode.Space)) {
                verticalVelocity += jumpVelocity;
            }

            if (!isGrounded) {
                verticalVelocity -= gravity * Time.deltaTime;
            }

            bool isFalling = verticalVelocity < defaultVerticalVelocity;
            if (isGrounded && isFalling) {
                verticalVelocity = defaultVerticalVelocity;
            }

            verticalMoveVector += new Vector3(0, verticalVelocity, 0);
            return verticalMoveVector;
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

        // private void OnTriggerEnter2D(Collider2D other) {
        //     Debug.Log($"Entering on trigger");
        //     if (other.gameObject.layer == LayerMask.NameToLayer(groundLayer)) {
        //         isGrounded = true;
        //     }
        // }
        //
        // private void OnTriggerExit2D(Collider2D other) {
        //     Debug.Log($"Exiting on trigger");
        //     if (other.gameObject.layer == LayerMask.NameToLayer(groundLayer)) {
        //         isGrounded = false;
        //     }
        // }
    }
}
