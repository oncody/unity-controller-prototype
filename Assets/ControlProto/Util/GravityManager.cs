using Unity.VisualScripting;
using UnityEngine;

namespace ControlProto.Util {
    public class GravityManager {
        private readonly float defaultVerticalVelocity;
        private readonly float jumpVelocity;
        private readonly LayerMask groundLayerMask;
        private readonly float floatTolerance;
        private readonly float gravity;
        private readonly float groundCheckDistance;

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

        public GravityManager(string groundLayer, float defaultVerticalVelocity, float jumpHeight, float gravity, float groundCheckDistance, float floatTolerance) {
            this.defaultVerticalVelocity = defaultVerticalVelocity;
            this.gravity = gravity;
            this.floatTolerance = floatTolerance;
            this.groundCheckDistance = groundCheckDistance;
            jumpVelocity = Mathf.Sqrt(2f * jumpHeight * gravity) - defaultVerticalVelocity;
            groundLayerMask = 1 << LayerMask.NameToLayer(groundLayer);
            verticalVelocity = defaultVerticalVelocity;
        }

        private void PerformGroundCheck(CharacterController playerController, Transform playerTransform) {
            // lets use a checkcapsule
            // start it at the bottom of the player
            // Vector3 rayOrigin = CharacterControllers.GetBottom(playerController, playerTransform);
            // Vector3 rayEnd = rayOrigin - new Vector3(0, groundCheckDistance, 0);
            // bool currentlyGrounded = Physics.CheckCapsule(playerPosition, rayEnd, playerController.radius, groundLayerMask, QueryTriggerInteraction.Ignore);
            // if (currentlyGrounded != isGrounded) {
            // Debug.Log($"isGrounded: {currentlyGrounded}");
            // }

            // isGrounded = currentlyGrounded;
            // if (jumpStarted && !jumpLeftGround && !isGrounded) {
            // jumpLeftGround = true;
            // }

            // if (jumpStarted && jumpLeftGround && isGrounded) {
            // jumpStarted = false;
            // jumpLeftGround = false;
            // }
        }

        public void UpdatePlayerPosition(Vector3 currentPosition) {
            // isFallingVertically = Mathf.Abs(currentPosition.y - playerPosition.y) > floatTolerance && currentPosition.y < playerPosition.y;
            // if (!startedFallingVertically && isFallingVertically) {
            // startedFallingVertically = true;
            // finishedFallingVertically = false;
            // }

            // if (startedFallingVertically && !isFallingVertically) {
            // startedFallingVertically = false;
            // finishedFallingVertically = true;
            // }

            // playerPosition = currentPosition;
        }

        public void JumpRequested(CharacterController playerController, Transform playerTransform) {
            // if (CurrentlyJumping() || CurrentlyFalling()) {
            //     return;
            // }
            //
            // PerformGroundCheck(playerController, playerTransform);
            // if (isGrounded) {
            //     jumpStarted = true;
            //     jumpLeftGround = false;
            //     verticalVelocity += jumpVelocity;
            // }
        }

        private bool CurrentlyJumping() {
            // return jumpStarted || jumpLeftGround;
            return false;
        }

        private bool CurrentlyFalling() {
            // return startedFallingVertically && !finishedFallingVertically;
            return false;
        }

        public Vector3 CalculateVerticalMovement(CharacterController playerController, Transform playerTransform) {
            // PerformGroundCheck(playerController, playerTransform);
            //
            // shouldFallBecauseOfGravity = false;
            //
            // // verticalVelocity < defaultVerticalVelocity ??
            // if (isGrounded && !CurrentlyJumping() && !CurrentlyFalling()) {
            //     verticalVelocity = defaultVerticalVelocity;
            //     return Vector3.zero;
            // }
            //
            // if (!isGrounded) {
            //     verticalVelocity -= gravity * Time.deltaTime;
            //     shouldFallBecauseOfGravity = true;
            //     // Debug.Log($"Adding gravity: {verticalVelocity}");
            // }
            //
            // return new Vector3(0, verticalVelocity, 0);
            return Vector3.zero;
        }
    }
}
