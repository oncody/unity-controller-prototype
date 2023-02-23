namespace ControlProto.Util.Input.Keyboard {
    public class KeyboardHandler {
        public bool IsActionPressed(KeyboardAction action) {
            return UnityEngine.Input.GetKeyDown(KeyboardActionMapper.GetMapping(action));
        }
    }
}
