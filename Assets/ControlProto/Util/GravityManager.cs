using UnityEngine;

namespace ControlProto.Util {
    public class GravityManager {
        private readonly float gravity;
        private readonly float floatTolerance;
        private readonly float defaultVelocity;
        private float lastPosition;
        private float velocity;
        private bool fallStarted;
        private bool fallFinished = true;

        public GravityManager(float gravity, float floatTolerance, float defaultVelocity, float position) {
            this.gravity = gravity;
            this.floatTolerance = floatTolerance;
            this.defaultVelocity = defaultVelocity;
            velocity = defaultVelocity;
            lastPosition = position;
        }

        public void UpdateFallingCheck(float position) {
            bool isFallingVertically = (Mathf.Abs(position - lastPosition) > floatTolerance) && (position < lastPosition);
            if (isFallingVertically && !fallStarted) {
                fallStarted = true;
                fallFinished = false;
            }

            if (!isFallingVertically && fallStarted) {
                fallStarted = false;
                fallFinished = true;
            }

            lastPosition = position;
        }

        private bool CurrentlyFalling() {
            return fallStarted && !fallFinished;
        }

        public float CalculateVerticalMovement() {
            if (!CurrentlyFalling()) {
                velocity = defaultVelocity;
                return 0;
            }

            velocity -= gravity * Time.deltaTime;
            return velocity;
        }
    }
}
