using ControlProto.Util.Gravity;
using UnityEngine;

namespace ControlProto.Util.PlayerController.FirstPerson {
    public class FirstPersonThreeDimensionalMovement {
        private readonly FirstPersonTwoDimensionalMovement firstPersonTwoDimensionalMovement;
        private readonly GravityManager gravityManager;

        public FirstPersonThreeDimensionalMovement(FirstPersonTwoDimensionalMovement firstPersonTwoDimensionalMovement, GravityManager gravityManager) {
            this.firstPersonTwoDimensionalMovement = firstPersonTwoDimensionalMovement;
            this.gravityManager = gravityManager;
        }

        public Vector3 Value(Transform player) {
            gravityManager.UpdateFallingCheck(player);

            Vector3 movementVector = firstPersonTwoDimensionalMovement.Value(player);
            float verticalMoveValue = gravityManager.CalculateVerticalMovement();

            // if we have horizontal movement, then we might move them off a ledge close to the ground. add a small amount of gravity to pull them down in case.
            if (movementVector != Vector3.zero && verticalMoveValue == 0) {
                verticalMoveValue = gravityManager.GravityConstants.DefaultVerticalVelocity;
            }

            return movementVector + new Vector3(0, verticalMoveValue, 0);
        }
    }
}
