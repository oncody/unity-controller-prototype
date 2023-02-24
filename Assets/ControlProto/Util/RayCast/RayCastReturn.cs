using UnityEngine;

namespace ControlProto.Util.RayCast {
    public class RayCastReturn {
        public bool EncounteredObject { get; private set; }
        public RaycastHit Hit { get; private set; }

        public RayCastReturn(bool encounteredObject, RaycastHit hit) {
            EncounteredObject = encounteredObject;
            Hit = hit;
        }
    }
}
