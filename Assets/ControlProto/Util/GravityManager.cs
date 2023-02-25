using UnityEngine;

namespace ControlProto.Util {
    public class GravityManager {
        private readonly float defaultVerticalVelocity;
        private readonly float jumpVelocity;
        private readonly float rayDistance;
        private readonly LayerMask groundLayerMask;
        private readonly float floatTolerance;
        private readonly float gravity;
        private readonly float playerRadius;

        private float verticalVelocity;
        private RaycastHit groundedSphereHit;
        private Vector3 playerPosition;

        private bool jumpStarted;
        private bool jumpLeftGround;
        private bool isFallingVertically;
        private bool isGrounded;
        private bool shouldFallBecauseOfGravity;
        private bool startedFallingVertically;
        private bool finishedFallingVertically = true;

        public GravityManager(string groundLayer, float defaultVerticalVelocity, float playerHeight, float jumpHeight, float gravity, float groundCheckDistance, float floatTolerance, float playerRadius) {
            this.defaultVerticalVelocity = defaultVerticalVelocity;
            this.gravity = gravity;
            this.floatTolerance = floatTolerance;
            verticalVelocity = defaultVerticalVelocity;
            jumpVelocity = Mathf.Sqrt(2f * jumpHeight * gravity) - defaultVerticalVelocity;
            groundLayerMask = 1 << LayerMask.NameToLayer(groundLayer);
            rayDistance = playerHeight / 2.0f + groundCheckDistance;
            this.playerRadius = playerRadius;
        }

        private void PerformGroundCheck() {
            bool currentlyGrounded = Physics.SphereCast(playerPosition, playerRadius, Vector3.down, out groundedSphereHit, rayDistance, groundLayerMask, QueryTriggerInteraction.Ignore);
            if (currentlyGrounded != isGrounded) {
                Debug.Log($"isGrounded: {currentlyGrounded}");
            }

            isGrounded = currentlyGrounded;

            if (jumpStarted && !jumpLeftGround && !isGrounded) {
                jumpLeftGround = true;
            }

            if (jumpStarted && jumpLeftGround && isGrounded) {
                jumpStarted = false;
                jumpLeftGround = false;
            }
        }

        public void UpdatePlayerPosition(Vector3 currentPosition) {
            isFallingVertically = Mathf.Abs(currentPosition.y - playerPosition.y) > floatTolerance && currentPosition.y < playerPosition.y;
            if (!startedFallingVertically && isFallingVertically) {
                startedFallingVertically = true;
                finishedFallingVertically = false;
            }

            if (startedFallingVertically && !isFallingVertically) {
                startedFallingVertically = false;
                finishedFallingVertically = true;
            }

            playerPosition = currentPosition;
        }

        public void JumpRequested() {
            if (CurrentlyJumping() || CurrentlyFalling()) {
                return;
            }

            PerformGroundCheck();
            if (isGrounded) {
                jumpStarted = true;
                jumpLeftGround = false;
                verticalVelocity += jumpVelocity;
            }
        }

        private bool CurrentlyJumping() {
            return jumpStarted || jumpLeftGround;
        }

        private bool CurrentlyFalling() {
            return startedFallingVertically && !finishedFallingVertically;
        }

        public Vector3 CalculateVerticalMovement() {
            PerformGroundCheck();

            shouldFallBecauseOfGravity = false;

            // verticalVelocity < defaultVerticalVelocity ??
            if (isGrounded && !CurrentlyJumping() && !CurrentlyFalling()) {
                verticalVelocity = defaultVerticalVelocity;
                return Vector3.zero;
            }

            if (!isGrounded) {
                verticalVelocity -= gravity * Time.deltaTime;
                shouldFallBecauseOfGravity = true;
            }

            return new Vector3(0, verticalVelocity, 0);
        }
    }
}
