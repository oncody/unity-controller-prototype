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

            // Set the distance of the ray to be slightly larger than the character's height
            float rayDistance = (characterController.height / 2.0f) + Maths.PositiveValue(globals.GroundCheckDistance);
            isGrounded = Physics.SphereCast(currentTransform.position, characterController.radius, Vector3.down, out _, rayDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            // Debug.Log($"isGrounded: {isGrounded}");

            if (isGrounded && velocity.y < 0) {
                velocity.y = -2;
            }

            float horizontalMovementValue = controllerHandler.HorizontalMovementInput();
            float verticalMovementValue = controllerHandler.VerticalMovementInput();

            Vector3 directionToMoveTo = currentTransform.right * horizontalMovementValue + currentTransform.forward * verticalMovementValue;
            characterController.Move(directionToMoveTo * (Maths.PositiveValue(globals.DefaultMovementSpeed) * Time.deltaTime));

            velocity.y -= Maths.PositiveValue(globals.Gravity) * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);
        }
    }
}
