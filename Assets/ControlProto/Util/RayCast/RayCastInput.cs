using UnityEngine;

namespace ControlProto.Util.RayCast {
    public class RayCastInput {
        public Vector3 Origin { get; }
        public Vector3 Direction { get; }
        public float Distance { get; }

        public RayCastInput(Vector3 origin, Vector3 direction, float distance) {
            Origin = origin;
            Direction = direction;
            Distance = distance;
        }
    }
}
