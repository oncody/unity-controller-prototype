using UnityEngine.InputSystem;

namespace ControlProto.Util.PlayerInputSystem.New {
    public class NewPlayerInputSystem : IPlayerInputSystem {
        private readonly DefaultInputActions defaultInputActions;
        private readonly IPlayerVector2Input lookInput;
        private readonly IPlayerVector2Input moveInput;
        private readonly IPlayerToggleInput crouchInput;
        private readonly IPlayerToggleInput sprintInput;

        public NewPlayerInputSystem(DefaultInputActions defaultInputActions) {
            this.defaultInputActions = defaultInputActions;
            lookInput = new NewLookInput(this.defaultInputActions);
            moveInput = new NewMoveInput(this.defaultInputActions);
            crouchInput = new NewCrouchInput();
            sprintInput = new NewSprintInput();
            defaultInputActions.Enable();
        }

        public IPlayerVector2Input LookInput() {
            return lookInput;
        }

        public IPlayerVector2Input MoveInput() {
            return moveInput;
        }

        public IPlayerToggleInput CrouchInput() {
            return crouchInput;
        }

        public IPlayerToggleInput SprintInput() {
            return sprintInput;
        }
    }
}
