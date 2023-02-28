using ControlProto.Util.PlayerInputSystem;

namespace ControlProto.Util.TwoDimensionalGroundPlaneMovement {
    public class SpeedManager {
        private readonly IPlayerInputSystem inputSystem;
        private readonly Speeds speeds;

        public SpeedManager(IPlayerInputSystem inputSystem, Speeds speeds) {
            this.inputSystem = inputSystem;
            this.speeds = speeds;
        }

        public float Value() {
            if (inputSystem.CrouchInput()) {
                return speeds.Crouch;
            }

            return (inputSystem.SprintInput()) ? speeds.Sprint : speeds.Walk;
        }
    }
}
