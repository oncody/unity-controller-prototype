using ControlProto.Util.Gravity;
using UnityEngine;

namespace ControlProto.Util.PlayerController.ThirdPerson {
    public class ThirdPersonThreeDimensionalMovement : IPlayerMovement {
        private readonly CharacterController controller;
        private readonly Transform player;
        private readonly ThirdPersonTwoDimensionalMovement thirdPersonTwoDimensionalMovement;
        private readonly GravityManager gravityManager;

        public ThirdPersonThreeDimensionalMovement(CharacterController controller, Transform player, ThirdPersonTwoDimensionalMovement thirdPersonTwoDimensionalMovement, GravityManager gravityManager) {
            this.controller = controller;
            this.player = player;
            this.thirdPersonTwoDimensionalMovement = thirdPersonTwoDimensionalMovement;
            this.gravityManager = gravityManager;
        }

        public void MovePlayer() {
            Vector3 movementVector = MoveVector();
            if (movementVector != Vector3.zero) {
                // Debug.Log($"Moving player from: {transform.position} to: {moveVector}");
                controller.Move(movementVector * Time.deltaTime);
            }
        }

        private Vector3 MoveVector() {
            gravityManager.UpdateFallingCheck();

            Vector3 movementVector = thirdPersonTwoDimensionalMovement.Value();
            float verticalMoveValue = gravityManager.CalculateVerticalMovement();

            // if we have horizontal movement, then we might move them off a ledge close to the ground. add a small amount of gravity to pull them down in case.
            if (movementVector != Vector3.zero && verticalMoveValue == 0) {
                verticalMoveValue = gravityManager.GravityConstants.DefaultVerticalVelocity;
            }

            return movementVector + new Vector3(0, verticalMoveValue, 0);
        }
    }
}
