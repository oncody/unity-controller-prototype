using ControlProto.Util.Gravity;
using ControlProto.Util.Unity;
using UnityEngine;

namespace ControlProto.Util.PlayerController {
    public abstract class PlayerController {
        private readonly PlayerObjects playerObjects;
        private readonly GravityManager gravityManager;

        public PlayerController(PlayerObjects playerObjects, GravityManager gravityManager) {
            this.playerObjects = playerObjects;
            this.gravityManager = gravityManager;
        }

        public void Update() {
            RotateCamera();
            RotatePlayer();
            MovePlayer();
        }

        public void MovePlayer() {
            Vector3 movementVector = MoveVector();
            if (movementVector != Vector3.zero) {
                // Debug.Log($"Moving player from: {transform.position} to: {moveVector}");
                playerObjects.Controller.Move(movementVector * Time.deltaTime);
            }
        }

        private Vector3 MoveVector() {
            gravityManager.UpdateFallingCheck();

            Vector3 movementVector = TwoDimensionalMovement();
            float verticalMoveValue = gravityManager.CalculateVerticalMovement();

            // if we have horizontal movement, then we might move them off a ledge close to the ground. add a small amount of gravity to pull them down in case.
            if (movementVector != Vector3.zero && verticalMoveValue == 0) {
                verticalMoveValue = gravityManager.GravityConstants.DefaultVerticalVelocity;
            }

            return movementVector + new Vector3(0, verticalMoveValue, 0);
        }

        // public abstract void CreateCamera();
        public abstract void RotateCamera();
        public abstract void RotatePlayer();

        // Maybe make this Vector2 in the future
        public abstract Vector3 TwoDimensionalMovement();
    }
}
