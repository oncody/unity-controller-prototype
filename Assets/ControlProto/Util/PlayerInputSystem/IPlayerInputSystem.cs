namespace ControlProto.Util.PlayerInputSystem {
    public interface IPlayerInputSystem {
        public IPlayerVector2Input LookInput();
        public IPlayerVector2Input MoveInput();
        public IPlayerToggleInput CrouchInput();
        public IPlayerToggleInput SprintInput();
    }
}
