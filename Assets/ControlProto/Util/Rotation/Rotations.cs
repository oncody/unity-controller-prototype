using UnityEngine;

namespace ControlProto.Util.Rotation {
    public static class Rotations {
        //todo: i might need *= here sometimes??
        public static void RotateLocally(Transform transform, RotationAxis axis, float angle) {
            transform.localRotation = Quaternion.AngleAxis(angle, RotationAxisMapper.GetMapping(axis));
        }

        //todo: i might need *= here sometimes??
        public static void RotateGlobally(Transform transform, RotationAxis axis, float angle) {
            transform.rotation = Quaternion.AngleAxis(angle, RotationAxisMapper.GetMapping(axis));
        }
    }
}
