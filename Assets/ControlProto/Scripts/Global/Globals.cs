using UnityEngine;

namespace ControlProto.Scripts.Global {
    public class Globals : MonoBehaviour {
        [field: Header("Camera")]
        [field: SerializeField]
        [field: Tooltip("This controls how sensitive the mouse moves vertically")]
        public float VerticalMouseSensitivity { get; private set; }

        [field: SerializeField]
        [field: Tooltip("This controls how sensitive the mouse moves horizontally")]
        public float HorizontalMouseSensitivity { get; private set; }

        [field: Header("Movement")]
        [field: SerializeField]
        [field: Tooltip("This is how far away from the bottom of the character to search for the ground in meters")]
        public float GroundCheckDistance { get; private set; }

        [field: SerializeField]
        [field: Tooltip("This is how fast the player moves in meters per second")]
        public float CrouchMovementSpeed { get; private set; }

        [field: SerializeField]
        [field: Tooltip("This is how fast the player moves in meters per second")]
        public float WalkMovementSpeed { get; private set; }

        [field: SerializeField]
        [field: Tooltip("This is how fast the player moves in meters per second")]
        public float SprintMovementSpeed { get; private set; }

        [field: SerializeField]
        [field: Tooltip("This is how strong gravity is in meters per second squared")]
        public float Gravity { get; private set; }

        [field: SerializeField]
        [field: Tooltip("This is how high to jump measured in meters")]
        public float JumpHeight { get; private set; }

        [field: SerializeField]
        [field: Tooltip("Whether or not to run the game in debug mode. This enables some logging")]
        public bool DebugEnabled { get; private set; }
    }
}
