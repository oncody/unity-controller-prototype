using ControlProto.Util.PlayerInputSystem;
using ControlProto.Util.TwoDimensionalGroundPlaneMovement;
using UnityEngine;

namespace ControlProto.Util.PlayerController.ThirdPerson {
    public class ThirdPersonTwoDimensionalMovement {
        private readonly IPlayerInputSystem inputSystem;
        private readonly Transform player;
        private readonly Transform virtualCamera;
        private readonly Transform sceneCamera;
        private readonly SpeedManager speedManager;

        public ThirdPersonTwoDimensionalMovement(IPlayerInputSystem inputSystem, Transform player, Transform virtualCamera, Transform sceneCamera, SpeedManager speedManager) {
            this.inputSystem = inputSystem;
            this.player = player;
            this.virtualCamera = virtualCamera;
            this.sceneCamera = sceneCamera;
            this.speedManager = speedManager;
        }

        // For now this will return a vector 3. Weird but world move direction needs this to be a vector 3 or this will do weird thing. Move the user vertically
        public Vector3 Value() {
            if (inputSystem.MoveInput().Value() == Vector2.zero) {
                return Vector3.zero;
            }

            float horizontalInput = inputSystem.MoveInput().Value().x;
            float verticalInput = inputSystem.MoveInput().Value().y;
            Vector3 localMoveDirection = new Vector3(horizontalInput, 0, verticalInput);

            if (localMoveDirection.magnitude >= 0.1f) {
                float targetangle = Mathf.Atan2(localMoveDirection.x, localMoveDirection.z) * Mathf.Rad2Deg + sceneCamera.eulerAngles.y;
                // float angle = Mathf.SmoothDampAngle(player.eulerAngles.y, targetangle, ref turnVelocity, turnSmoothTime);
                player.rotation = Quaternion.Euler(0f, targetangle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetangle, 0f) * Vector3.forward;
                return moveDir.normalized * speedManager.Value();
            }

            return Vector3.zero;
        }
    }
}
