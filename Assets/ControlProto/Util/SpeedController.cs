using ControlProto.Util.Input.Keyboard;

namespace ControlProto.Util {
    public class SpeedController {
        private readonly KeyboardHandler keyboardHandler = new();

        private float crouchMovementSpeed;
        private float walkMovementSpeed;
        private float sprintMovementSpeed;

        public SpeedController(float crouchMovementSpeed, float walkMovementSpeed, float sprintMovementSpeed) {
            this.crouchMovementSpeed = Maths.PositiveValue(crouchMovementSpeed);
            this.walkMovementSpeed = Maths.PositiveValue(walkMovementSpeed);
            this.sprintMovementSpeed = Maths.PositiveValue(sprintMovementSpeed);
        }

        public float MovementSpeed() {
            if (keyboardHandler.IsActionPressed(KeyboardAction.Crouch)) {
                return crouchMovementSpeed;
            }

            return keyboardHandler.IsActionPressed(KeyboardAction.Sprint) ? sprintMovementSpeed : walkMovementSpeed;
        }
    }
}
