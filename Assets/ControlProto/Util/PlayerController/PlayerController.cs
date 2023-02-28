namespace ControlProto.Util.PlayerController {
    public abstract class PlayerController {
        public void Update() {
            CameraRotation().Rotate();
            PlayerRotation().Rotate();
            PlayerMovement().MovePlayer();
        }

        public abstract ICameraRotation CameraRotation();
        public abstract IPlayerMovement PlayerMovement();
        public abstract IPlayerRotation PlayerRotation();
    }
}
