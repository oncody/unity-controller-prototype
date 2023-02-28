namespace ControlProto.Util.PlayerRotation {
    public class PitchBounds {
        public float Min { get; }
        public float Max { get; }

        public PitchBounds(float min, float max) {
            Min = min;
            Max = max;
        }
    }
}
