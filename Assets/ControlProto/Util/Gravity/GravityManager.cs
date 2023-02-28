using UnityEngine;

namespace ControlProto.Util.Gravity {
    public class GravityManager {
        public GravityConstants GravityConstants { get; }
        private readonly Transform player;
        private float lastYPosition;
        private float velocity;
        private bool fallStarted;
        private bool fallFinished = true;

        public GravityManager(GravityConstants gravityConstants, Transform player) {
            GravityConstants = gravityConstants;
            this.player = player;
            velocity = gravityConstants.DefaultVerticalVelocity;
            lastYPosition = player.position.y;
        }

        public void UpdateFallingCheck() {
            float currentYPosition = player.position.y;
            bool isFallingVertically = (Mathf.Abs(currentYPosition - lastYPosition) > GravityConstants.FloatTolerance) && (currentYPosition < lastYPosition);
            if (isFallingVertically && !fallStarted) {
                fallStarted = true;
                fallFinished = false;
            }

            if (!isFallingVertically && fallStarted) {
                fallStarted = false;
                fallFinished = true;
            }

            lastYPosition = currentYPosition;
        }

        private bool CurrentlyFalling() {
            return fallStarted && !fallFinished;
        }

        public float CalculateVerticalMovement() {
            if (!CurrentlyFalling()) {
                velocity = GravityConstants.DefaultVerticalVelocity;
                return 0;
            }

            velocity -= GravityConstants.Gravity * Time.deltaTime;
            return velocity;
        }
    }
}
