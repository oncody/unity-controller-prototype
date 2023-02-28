namespace ControlProto.Util.TwoDimensionalGroundPlaneMovement {
    public class Speeds {
        public float Crouch { get; }
        public float Walk { get; }
        public float Sprint { get; }

        public Speeds(float crouch, float walk, float sprint) {
            Crouch = crouch;
            Walk = walk;
            Sprint = sprint;
        }
    }
}