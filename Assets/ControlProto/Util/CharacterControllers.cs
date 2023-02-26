using UnityEngine;

namespace ControlProto.Util {
    public static class CharacterControllers {
        public static Vector3 GetTop(CharacterController controller, Transform transform) {
            return transform.position + new Vector3(0, controller.height / 2, 0);
        }

        public static Vector3 GetBottom(CharacterController controller, Transform transform) {
            return transform.position - new Vector3(0, controller.height / 2, 0);
        }
    }
}
