using UnityEngine;

namespace ControlProto.Util.RayCast {
    public static class Raycasts {
        public static RayCastReturn SphereCast(SphereCastInput input, int layerMask, QueryTriggerInteraction interaction) {
            // Start the raycast at the center point of our character
            // Set the max ray distance to be slightly larger than half of the character's height
            bool encounteredObject = Physics.SphereCast(input.RayInput.Origin, input.Radius, input.RayInput.Direction, out var hit, input.RayInput.Distance, layerMask, interaction);
            return new RayCastReturn(encounteredObject, hit);
        }

        public static RayCastReturn RayCast(RayCastInput input, int layerMask, QueryTriggerInteraction interaction) {
            // Start the raycast at the center point of our character
            // Set the max ray distance to be slightly larger than half of the character's height
            bool encounteredObject = Physics.Raycast(input.Origin, input.Direction, out var hit, input.Distance, layerMask, interaction);
            return new RayCastReturn(encounteredObject, hit);
        }
    }
}
