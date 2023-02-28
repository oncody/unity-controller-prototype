namespace ControlProto.Util.Unity {
    public class PlayerLookConstants {
        public float HorizontalMouseSensitivity { get; }
        public float VerticalMouseSensitivity { get; }
        public float MinPitch { get; }
        public float MaxPitch { get; }

        public PlayerLookConstants(float horizontalMouseSensitivity, float verticalMouseSensitivity, float minPitch, float maxPitch) {
            HorizontalMouseSensitivity = horizontalMouseSensitivity;
            VerticalMouseSensitivity = verticalMouseSensitivity;
            MinPitch = minPitch;
            MaxPitch = maxPitch;
        }
    }
}
