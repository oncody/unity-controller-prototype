namespace ControlProto.Util.RayCast {
    public class SphereCastInput {
        public RayCastInput RayInput { get; }
        public float Radius { get; }

        public SphereCastInput(RayCastInput rayInput, float radius) {
            RayInput = rayInput;
            Radius = radius;
        }
    }
}
