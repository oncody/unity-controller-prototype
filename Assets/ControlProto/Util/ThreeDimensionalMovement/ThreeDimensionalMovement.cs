using ControlProto.Util.Gravity;
using ControlProto.Util.TwoDimensionalGroundPlaneMovement;
using UnityEngine;

namespace ControlProto.Util.ThreeDimensionalMovement {
    public class ThreeDimensionalMovement {
        private readonly TwoDimensionalMovement twoDimensionalMovement;
        private readonly GravityManager gravityManager;

        public ThreeDimensionalMovement(TwoDimensionalMovement twoDimensionalMovement, GravityManager gravityManager) {
            this.twoDimensionalMovement = twoDimensionalMovement;
            this.gravityManager = gravityManager;
        }

        public Vector3 Value(Transform player) {
            gravityManager.UpdateFallingCheck(player);

            Vector3 movementVector = twoDimensionalMovement.Value(player);
            float verticalMoveValue = gravityManager.CalculateVerticalMovement();

            // if we have horizontal movement, then we might move them off a ledge close to the ground. add a small amount of gravity to pull them down in case.
            if (movementVector != Vector3.zero && verticalMoveValue == 0) {
                verticalMoveValue = gravityManager.GravityConstants.DefaultVerticalVelocity;
            }

            return movementVector + new Vector3(0, verticalMoveValue, 0);
        }
    }
}