using System.Collections.Generic;
using UnityEngine;

namespace ControlProto.Util.Rotation {
    public static class RotationAxisMapper {
        private static readonly Dictionary<RotationAxis, Vector3> RotationMap = new() {
            { RotationAxis.XAxis, Vector3.right },
            { RotationAxis.YAxis, Vector3.up },
            { RotationAxis.ZAxis, Vector3.forward },
        };

        public static Vector3 GetMapping(RotationAxis axis) {
            return RotationMap[axis];
        }
    }
}
