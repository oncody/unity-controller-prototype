using System;
using ControlProto.Scripts.Global;
using ControlProto.Util;
using ControlProto.Util.Input.Controller;
using UnityEngine;

namespace ControlProto.Scripts.Player {
    public class PlayerMovement : MonoBehaviour {
        [SerializeField] private Globals globals;
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Transform cameraTransform;

        private const float MaxVerticalLookAngle = 90;
        private const float MinVerticalLookAngle = 90;

        private readonly ControllerHandler controllerHandler = new();
        private float horizontalRotation = 0;

        private float horizontalMouseMovementValue;
        private float verticalMouseMovementValue;
        private Vector3 directionToMoveTo;

        private void Start() {
            // GatherInputs();
        }

        private void Update() {
            GatherInputs();
        }

        private void GatherInputs() {
            horizontalMouseMovementValue = controllerHandler.HorizontalMouseInput() * globals.HorizontalMouseSensitivity;
            verticalMouseMovementValue = controllerHandler.VerticalMouseInput() * globals.VerticalMouseSensitivity;

            // Rotate the player horizontally around the y-axis based on the mouse movement
            transform.Rotate(0, horizontalMouseMovementValue, 0);

            // Calculate the new rotation on the x-axis and clamp it to a range of -90 to 90 degrees
            horizontalRotation -= verticalMouseMovementValue;
            horizontalRotation = Mathf.Clamp(horizontalRotation, Maths.NegativeValue(MinVerticalLookAngle), Maths.PositiveValue(MaxVerticalLookAngle));

            // Rotate the camera around the x-axis based on the new rotation value
            cameraTransform.localRotation = Quaternion.Euler(horizontalRotation, 0, 0);

            // Calculate the movement direction based on input and the player's rotation
            directionToMoveTo = transform.forward * controllerHandler.VerticalMovementInput() + transform.right * controllerHandler.HorizontalMovementInput();

            // Move the character based on the movement direction and speed
            characterController.Move(directionToMoveTo * (Maths.PositiveValue(globals.DefaultMovementSpeed) * Time.deltaTime));
        }

        private void FixedUpdate() {
        }
    }
}
