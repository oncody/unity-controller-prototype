using UnityEngine;

namespace ControlProto.Util.PlayerInputSystem {
    public interface IPlayerInputSystem {
        public float HorizontalLookInput();
        public float VerticalLookInput();
        public float HorizontalMoveInput();
        public float VerticalLMoveInput();
        public bool CrouchInput();
        public bool SprintInput();
    }
}
