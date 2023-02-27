using UnityEngine;
using UnityEngine.InputSystem;

namespace ControlProto.Util {
    public class CharacterControllerMover {
        private readonly GravityManager gravityManager;
        private readonly TwoDimensionMovement twoDimensionMovement;
        private readonly CharacterController controller;
        private readonly float defaultVerticalVelocity;

        public CharacterControllerMover(
            CharacterController controller,
            DefaultInputActions defaultInputActions,
            Transform transform,
            float crouchMovementSpeed,
            float walkMovementSpeed,
            float sprintMovementSpeed,
            float gravity,
            float floatTolerance,
            float defaultVerticalVelocity) {
            gravityManager = new GravityManager(gravity, floatTolerance, defaultVerticalVelocity, transform.position.y);
            twoDimensionMovement = new TwoDimensionMovement(defaultInputActions, crouchMovementSpeed, walkMovementSpeed, sprintMovementSpeed);
            this.defaultVerticalVelocity = defaultVerticalVelocity;
            this.controller = controller;
        }

        public void MovePlayer(Transform transform) {
            gravityManager.UpdateFallingCheck(transform.position.y);

            Vector3 movementVector = twoDimensionMovement.CalculateTwoDimensionalMovement(transform);
            float verticalMoveValue = gravityManager.CalculateVerticalMovement();

            // if we have horizontal movement, then we might move them off a ledge close to the ground. add a small amount of gravity to pull them down in case.
            if (movementVector != Vector3.zero && verticalMoveValue == 0) {
                verticalMoveValue = defaultVerticalVelocity;
            }

            movementVector += new Vector3(0, verticalMoveValue, 0);
            if (movementVector != Vector3.zero) {
                // Debug.Log($"Moving player from: {transform.position} to: {moveVector}");
                controller.Move(movementVector * Time.deltaTime);
            }
        }
    }
}
