namespace ControlProto.Util.Input.Keyboard {
    public class KeyboardHandler {
        public bool IsActionPressed(KeyboardAction action) {
            return KeyboardActionMapper.GetMapping(action).isPressed;
        }
    }
}
