using ControlProto.Scripts.Global;
using ControlProto.Util;
using ControlProto.Util.Input.Controller;
using UnityEngine;

namespace ControlProto.Scripts.Player {
    public class PlayerMovement : MonoBehaviour {
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Globals globals;
        private readonly ControllerHandler controllerHandler = new();

        void Start() {
        }

        void Update() {
            Transform currentTransform = transform;
            Vector3 directionToMoveTo = currentTransform.right * controllerHandler.HorizontalMovementInput() + currentTransform.forward * controllerHandler.VerticalMovementInput();
            characterController.Move(directionToMoveTo * (Maths.PositiveValue(globals.DefaultMovementSpeed) * Time.deltaTime));
        }
    }
}
