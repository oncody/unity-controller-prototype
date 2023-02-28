using System;
using UnityEngine;

namespace ControlProto.Util.Unity {
    public class PlayerObjects {
        public CharacterController Controller { get; }
        public Transform Player { get; }
        public Transform Camera { get; }

        public PlayerObjects(CharacterController controller, Transform player, Transform camera) {
            Controller = controller;
            Player = player;
            Camera = camera;
        }
    }
}
