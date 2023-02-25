using UnityEngine;

namespace ControlProto.Util.RayCast {
    public class RayCastReturn {
        public bool EncounteredObject { get; }
        public RaycastHit Hit { get; }

        public RayCastReturn(bool encounteredObject, RaycastHit hit) {
            EncounteredObject = encounteredObject;
            Hit = hit;
        }
    }
}
