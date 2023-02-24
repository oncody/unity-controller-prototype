using UnityEngine;

namespace ControlProto.Util.RayCast {
    public class RayCastInput {
        public Vector3 Origin { get; private set; }
        public Vector3 Direction { get; private set; }
        public float Distance { get; private set; }

        public RayCastInput(Vector3 origin, Vector3 direction, float distance) {
            Origin = origin;
            Direction = direction;
            Distance = distance;
        }
    }
}
