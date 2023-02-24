using UnityEngine;

namespace ControlProto.Util {
    public class GravityController {
        private const float resetVelocity = 0f;

        private readonly float gravity;
        private readonly float jumpHeight = 0;

        // We need to keep track of this because we need to keep accelerating if they're falling because of gravity
        public float Velocity { get; private set; }

        /**
         * Gravity Value is in meters per second square
         */
        public GravityController(float gravity, float jumpHeight) {
            this.gravity = Maths.PositiveValue(gravity);
            this.jumpHeight = Maths.PositiveValue(jumpHeight);
            Velocity = 0;
        }

        public Vector3 MoveVector() {
            return new Vector3(0, Velocity, 0);
        }

        public void MoveForwardInTime(bool grounded, float deltaTime) {
            if (!grounded) {
                ApplyGravity(deltaTime);
            }

            if (grounded && IsMoving()) {
                ResetVelocity();
            }
        }

        public void ApplyJump() {
            Velocity += CalculateJumpVelocity();
        }

        private bool IsMoving() {
            return Velocity < 0;
        }

        private void ResetVelocity() {
            Velocity = resetVelocity;
        }

        private void ApplyGravity(float deltaTime) {
            Velocity -= gravity * deltaTime;
        }

        private float CalculateJumpVelocity() {
            return Mathf.Sqrt(2f * jumpHeight * gravity);
        }
    }
}
