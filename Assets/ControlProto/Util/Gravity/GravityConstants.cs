namespace ControlProto.Util {
    public class GravityConstants {
        public float Gravity { get; }
        public float DefaultVerticalVelocity { get; }
        public float FloatTolerance { get; }

        public GravityConstants(float gravity, float defaultVerticalVelocity, float floatTolerance) {
            Gravity = gravity;
            DefaultVerticalVelocity = defaultVerticalVelocity;
            FloatTolerance = floatTolerance;
        }
    }
}
