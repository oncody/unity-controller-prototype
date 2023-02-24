namespace ControlProto.Util.RayCast {
    public class SphereCastInput {
        public RayCastInput RayInput { get; private set; }
        public float Radius { get; private set; }

        public SphereCastInput(RayCastInput rayInput, float radius) {
            RayInput = rayInput;
            Radius = radius;
        }
    }
}
