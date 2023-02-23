using System;
using ControlProto.Scripts.Global;
using ControlProto.Util;
using ControlProto.Util.Input.Controller;
using ControlProto.Util.Rotation;
using UnityEngine;

namespace ControlProto.Scripts.Player {
    public class PlayerMovement : MonoBehaviour {
        [SerializeField] private Globals globals;
        [SerializeField] private CharacterController characterController;

        private readonly ControllerHandler controllerHandler = new();
        private float verticalVelocity = 0;
        private bool grounded;

        private void Update() {
            UpdateGrounded();
            MovePlayer();
        }

        private void MovePlayer() {
            UpdateVerticalVelocity();

            Vector3 finalHorizontalMovement = Vector3.zero;
            Vector3 relativeMoveDirection = GetKeyboardMovementAsVector3();
            if (relativeMoveDirection != Vector3.zero) {
                // Convert relative move direction to global move direction
                Vector3 absoluteMoveDirection = transform.TransformDirection(relativeMoveDirection);
                Vector3 normalizedAbsoluteMoveDirection = absoluteMoveDirection.normalized;
                finalHorizontalMovement = absoluteMoveDirection * globals.DefaultMovementSpeed;
            }

            Vector3 verticalMovement = new Vector3(0, verticalVelocity, 0);
            Vector3 combinedMovement = finalHorizontalMovement + verticalMovement;
            characterController.Move(combinedMovement * Time.deltaTime);
        }

        private void UpdateGrounded() {
            // Start the raycast at the center point of our character
            // Set the max ray distance to be slightly larger than half of the character's height
            float rayDistance = (characterController.height / 2.0f) + Maths.PositiveValue(globals.GroundCheckDistance);
            grounded = Physics.SphereCast(transform.position, characterController.radius, Vector3.down, out _, rayDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore);
        }

        private Vector2 GetKeyboardMovementAsVector2() {
            var horizontalKeyboardMovement = controllerHandler.HorizontalKeyboardMovement();
            var verticalKeyboardMovement = controllerHandler.VerticalKeyboardMovement();
            // Debug.Log($"horizontalKeyboardMovement: {horizontalKeyboardMovement}");
            // Debug.Log($"verticalKeyboardMovement: {verticalKeyboardMovement}");

            return new Vector2(horizontalKeyboardMovement, verticalKeyboardMovement);
        }

        private Vector3 GetKeyboardMovementAsVector3() {
            var horizontalKeyboardMovement = controllerHandler.HorizontalKeyboardMovement();
            var verticalKeyboardMovement = controllerHandler.VerticalKeyboardMovement();
            // Debug.Log($"horizontalKeyboardMovement: {horizontalKeyboardMovement}");
            // Debug.Log($"verticalKeyboardMovement: {verticalKeyboardMovement}");

            return new Vector3(horizontalKeyboardMovement, 0, verticalKeyboardMovement);
        }

        private void UpdateVerticalVelocity() {
            if (characterController.isGrounded) {
                if (verticalVelocity < 0) {
                    verticalVelocity = -2f;
                }

                if (Input.GetButtonDown("Jump") && characterController.isGrounded) {
                    verticalVelocity = Mathf.Sqrt(Maths.PositiveValue(globals.JumpHeight) * 2f * Maths.PositiveValue(globals.Gravity));
                }
            }
            else {
                verticalVelocity += Maths.NegativeValue(globals.Gravity) * Time.deltaTime;
            }

            // Debug.Log($"verticalVelocity: {verticalVelocity}");
        }
    }
}
