using UnityEngine;

namespace ControlProto.Util.Rotation {
    public static class Rotations {
        public static void RotateRelativeToParent(Transform transform, RotationAxis axis, float angle, bool rotateRelativeToCurrentRotation) {
            Quaternion rotation = Quaternion.AngleAxis(angle, RotationAxisMapper.GetMapping(axis));
            if (rotateRelativeToCurrentRotation) {
                transform.localRotation *= rotation;
            }
            else {
                transform.localRotation = rotation;
            }
        }

        public static void RotateGlobally(Transform transform, RotationAxis axis, float angle, bool rotateRelativeToCurrentRotation) {
            Quaternion rotation = Quaternion.AngleAxis(angle, RotationAxisMapper.GetMapping(axis));
            if (rotateRelativeToCurrentRotation) {
                transform.rotation *= rotation;
            }
            else {
                transform.rotation = rotation;
            }
        }
    }
}
