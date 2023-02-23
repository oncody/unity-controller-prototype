using ControlProto.Scripts.Global;
using ControlProto.Util;
using ControlProto.Util.Input.Controller;
using UnityEngine;

namespace ControlProto.Scripts.Player {
    public class PlayerMovement : MonoBehaviour {
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Globals globals;
        private ControllerHandler controllerHandler;
        private Vector3 velocity;
        private bool isGrounded;

        void Start() {
            controllerHandler = new ControllerHandler();
        }

        void Update() {
            Transform currentTransform = transform;

            // Store the current position and time for the next frame
            Vector3 currentPosition = currentTransform.position;

            // Get the collider component
            // TODO: CODY change this to bind to a collider
            Collider currentCollider = GetComponent<Collider>();
            Bounds bounds = currentCollider.bounds;

            float topPosition = currentPosition.y + bounds.extents.y;
            float bottomPosition = currentPosition.y - bounds.extents.y;
            float characterHeight = topPosition - bottomPosition;

            // Debug.Log($"topPosition: {topPosition}");
            // Debug.Log($"bottomPosition: {bottomPosition}");
            // Debug.Log($"characterHeight: {characterHeight}");

            RaycastHit hit;

            // Set the distance of the ray to be slightly larger than the character's height
            float rayDistance = (characterHeight / 2.0f) + Maths.PositiveValue(globals.GroundCheckDistance);
            Debug.Log($"rayDistance: {rayDistance}");

            isGrounded = Physics.SphereCast(currentPosition, characterController.radius, Vector3.down, out hit, rayDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore);

            Debug.Log($"isGrounded: {isGrounded}");

            if (isGrounded && velocity.y < 0) {
                velocity.y = 0;
            }

            float horizontalMovementValue = controllerHandler.HorizontalMovementInput();
            float verticalMovementValue = controllerHandler.VerticalMovementInput();

            Vector3 directionToMoveTo = currentTransform.right * horizontalMovementValue + currentTransform.forward * verticalMovementValue;
            characterController.Move(directionToMoveTo * (Maths.PositiveValue(globals.DefaultMovementSpeed) * Time.deltaTime));

            velocity.y -= globals.Gravity * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);
        }

        public bool IsGrounded() {
            return isGrounded;
        }
    }
}
