using System;
using ControlProto.Scripts.Global;
using ControlProto.Util;
using ControlProto.Util.Input.Controller;
using ControlProto.Util.Rotation;
using UnityEngine;

namespace ControlProto.Scripts.Player {
    public class PlayerMovement : MonoBehaviour {
        [SerializeField] private Globals globals;
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Transform cameraTransform;

        private const float MaxVerticalLookAngle = 90;
        private const float MinVerticalLookAngle = 90;

        private readonly ControllerHandler controllerHandler = new();

        private float verticalVelocity = 0;

        // Pitch is up and down rotation of an object. Like tilting your head forward and backward
        private float pitch = 0.0f;

        // Yaw refers to left and right rotation of an object. For example, turning your head and looking left or right
        private float yaw = 0.0f;

        private void Update() {
            UpdateCameraRotation();
            Vector3 horizontalMovement = CalculateHorizontalMovement();
            // UpdateVerticalVelocity();
            // Vector3 finalVerticalMovement = new Vector3(0, verticalVelocity, 0) * Time.deltaTime;
            characterController.Move(horizontalMovement * (globals.DefaultMovementSpeed * Time.deltaTime));
        }

        private void FixedUpdate() {
        }

        private void LateUpdate() {
            // UpdateCameraRotation();
        }

        private void UpdateCameraRotation() {
            yaw += controllerHandler.HorizontalMouseMovement() * globals.HorizontalMouseSensitivity;
            pitch -= controllerHandler.VerticalMouseMovement() * globals.VerticalMouseSensitivity;
            pitch = Mathf.Clamp(pitch, Maths.NegativeValue(MinVerticalLookAngle), Maths.PositiveValue(MaxVerticalLookAngle));

            // Rotate camera up and down
            Rotations.RotateLocally(cameraTransform, RotationAxis.XAxis, pitch);

            // Rotate player left and right
            Rotations.RotateGlobally(transform, RotationAxis.YAxis, yaw);
        }

        private Vector3 CalculateHorizontalMovement() {
            Vector3 relativeMoveDirection = new Vector3(controllerHandler.HorizontalKeyboardMovement(), 0, controllerHandler.VerticalKeyboardMovement());

            // Convert relative move direction to global move direction
            return transform.TransformDirection(relativeMoveDirection);
        }

        private void UpdateVerticalVelocity() {
            if (Input.GetButtonDown("Jump") && characterController.isGrounded) {
                verticalVelocity = Mathf.Sqrt(Maths.PositiveValue(globals.JumpHeight) * 2f * Maths.PositiveValue(globals.Gravity));
            }

            verticalVelocity += Maths.NegativeValue(globals.Gravity) * Time.deltaTime;

            if (characterController.isGrounded && verticalVelocity < 0) {
                verticalVelocity = -2f;
            }
        }
    }
}
