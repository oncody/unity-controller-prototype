using UnityEngine;

namespace ControlProto.Util {
    public class GravityController {
        private const float resetVelocity = 0f;

        private readonly float gravity;
        private readonly float jumpHeight = 0;

        // We need to keep track of this because we need to keep accelerating if they're falling because of gravity
        private float velocity = 0;

        /**
         * Gravity Value is in meters per second square
         */
        public GravityController(float gravity, float jumpHeight) {
            this.gravity = Maths.PositiveValue(gravity);
            this.jumpHeight = Maths.PositiveValue(jumpHeight);
        }

        public void MoveForwardInTime(bool grounded, float deltaTime) {
            if (!grounded) {
                ApplyGravity(deltaTime);
            }

            if (grounded && IsMoving()) {
                ResetVelocity();
            }
        }

        public float GetVelocity() {
            return velocity;
        }

        public void ApplyJump() {
            velocity += CalculateJumpVelocity();
        }

        private bool IsMoving() {
            return velocity < 0;
        }

        private void ResetVelocity() {
            velocity = resetVelocity;
        }

        private void ApplyGravity(float deltaTime) {
            velocity -= gravity * deltaTime;
        }

        private float CalculateJumpVelocity() {
            return Mathf.Sqrt(2f * jumpHeight * gravity);
        }
    }
}
