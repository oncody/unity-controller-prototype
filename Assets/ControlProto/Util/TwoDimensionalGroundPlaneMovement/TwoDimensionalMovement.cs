using ControlProto.Util.PlayerInputSystem;
using UnityEngine;

namespace ControlProto.Util.TwoDimensionalGroundPlaneMovement {
    public class TwoDimensionalMovement {
        private readonly IPlayerInputSystem inputSystem;
        private readonly SpeedManager speedManager;

        public TwoDimensionalMovement(IPlayerInputSystem inputSystem, SpeedManager speedManager) {
            this.inputSystem = inputSystem;
            this.speedManager = speedManager;
        }

        // For now this will return a vector 3. Weird but world move direction needs this to be a vector 3 or this will do weird thing. Move the user vertically
        public Vector3 Value(Transform player) {
            if (inputSystem.MoveInput().Value() == Vector2.zero) {
                return Vector3.zero;
            }

            float horizontalInput = inputSystem.MoveInput().Value().x;
            float verticalInput = inputSystem.MoveInput().Value().y;
            Vector3 localMoveDirection = new Vector3(horizontalInput, 0, verticalInput);
            Vector3 worldMoveDirection = player.TransformDirection(localMoveDirection);
            return worldMoveDirection.normalized * speedManager.Value();
        }
    }
}