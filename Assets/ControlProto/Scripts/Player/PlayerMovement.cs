using System;
using ControlProto.Scripts.Global;
using ControlProto.Util;
using ControlProto.Util.Input.Controller;
using UnityEngine;

namespace ControlProto.Scripts.Player {
    public class PlayerMovement : MonoBehaviour {
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Globals globals;
        private readonly ControllerHandler controllerHandler = new();
        private Vector3 directionToMoveTo;

        private void Start() {
            // GatherInputs();
        }

        private void Update() {
            GatherInputs();
        }

        private void GatherInputs() {
            Transform currentTransform = transform;
            directionToMoveTo = currentTransform.right * controllerHandler.HorizontalMovementInput() + currentTransform.forward * controllerHandler.VerticalMovementInput();
        }

        private void FixedUpdate() {
            characterController.Move(directionToMoveTo * (Maths.PositiveValue(globals.DefaultMovementSpeed) * Time.deltaTime));
        }
    }
}
