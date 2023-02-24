namespace ControlProto.Util {
    public static class Maths {
        public static float PositiveValue(float value) {
            return value >= 0 ? value : value * -1f;
        }

        public static float NegativeValue(float value) {
            return value <= 0 ? value : value * -1f;
        }
    }
}
