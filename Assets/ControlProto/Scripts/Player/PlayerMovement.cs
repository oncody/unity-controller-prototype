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
        // private float horizontalRotation = 0;

        // private float horizontalMouseMovementValue;
        // private float verticalMouseMovementValue;
        // private Vector3 directionToMoveTo;

        private float verticalVelocity = 0;

        // Private variables for storing rotation and movement data
        // Pitch is up and down rotation of an object. Like tilting your head forward and backward
        private float pitch = 0.0f;

        // Yaw referes to left and right rotation of an object. For example, turning your head and looking left or right
        private float yaw = 0.0f;

        private void Start() {
            // GatherInputs();
        }

        private void Update() {
            // UpdateVerticalVelocity();

            // Vector2 horizontalMovement = CalculateHorizontalMovement();
            // Vector3 horizontalMovement3 = new Vector3(horizontalMovement.x, 0, horizontalMovement.y).normalized;
            // Vector3 finalHorizontalMovement = horizontalMovement3 * (globals.DefaultMovementSpeed * Time.deltaTime);
            // // Vector3 finalVerticalMovement = new Vector3(0, verticalVelocity, 0) * Time.deltaTime;
            //
            // Debug.Log($"X movement: {finalHorizontalMovement.x}");
            // Debug.Log($"Y movement: {finalHorizontalMovement.y}");
            // Debug.Log($"Z movement: {finalHorizontalMovement.z}");

            // characterController.Move(finalHorizontalMovement);
            UpdateCameraRotation();
            UpdateMovement();
        }

        private void FixedUpdate() {
        }

        private void LateUpdate() {
            // UpdateCameraRotation();
        }

        private void GatherInputs() {
            // horizontalMouseMovementValue += controllerHandler.HorizontalMouseInput();
            // verticalMouseMovementValue -= controllerHandler.VerticalMouseInput();
            // verticalMouseMovementValue = Mathf.Clamp(verticalMouseMovementValue, Maths.NegativeValue(MinVerticalLookAngle), Maths.PositiveValue(MaxVerticalLookAngle));

            // Rotate the player horizontally around the y-axis based on the mouse movement
            // transform.Rotate(0, horizontalMouseMovementValue, 0);

            // Calculate the new rotation on the x-axis and clamp it to a range of -90 to 90 degrees
            // horizontalRotation -= verticalMouseMovementValue;
            // horizontalRotation = Mathf.Clamp(horizontalRotation, Maths.NegativeValue(MinVerticalLookAngle), Maths.PositiveValue(MaxVerticalLookAngle));

            // Rotate the camera around the x-axis based on the new rotation value
            // cameraTransform.localRotation = Quaternion.Euler(horizontalRotation, 0, 0);
            // transform.localRotation = Quaternion.Euler(verticalMouseMovementValue, horizontalMouseMovementValue, 0);
        }

        private void UpdateCameraRotation() {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // Update the yaw and pitch angles based on mouse input
            yaw += mouseX * globals.HorizontalMouseSensitivity;
            pitch -= mouseY * globals.VerticalMouseSensitivity;
            pitch = Mathf.Clamp(pitch, -90f, 90f);

            // Rotate the camera based on pitch angle
            Rotations.RotateLocally(cameraTransform, RotationAxis.XAxis, pitch);

            // Rotate the player based on yaw angle
            Rotations.RotateGlobally(transform, RotationAxis.YAxis, yaw);


            // cameraTransform.localRotation = Quaternion.Euler(mouseY, 0, 0);
            // transform.Rotate(Vector3.up * mouseX);
        }

        private void UpdateMovement() {
            // Get input values for movement and rotation
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            // Calculate movement direction based on input values
            Vector3 relativeMoveDirection = new Vector3(horizontal, 0, vertical);
            Vector3 globalMoveDirection = transform.TransformDirection(relativeMoveDirection);

            // Apply movement to character controller
            characterController.Move(globalMoveDirection * (globals.DefaultMovementSpeed * Time.deltaTime));
        }

        private void UpdateVerticalVelocity() {
            // if (Input.GetButtonDown("Jump") && characterController.isGrounded) {
            // velocity.y = Mathf.Sqrt(Maths.PositiveValue(globals.JumpHeight) * 2f * Maths.PositiveValue(globals.Gravity));
            // }

            // verticalVelocity += Maths.NegativeValue(globals.Gravity) * Time.deltaTime;

            // if (characterController.isGrounded && verticalVelocity < 0) {
            // verticalVelocity = -2f;
            // }
        }

        // private Vector2 CalculateHorizontalMovement() {
        // float horizontalInput = Input.GetAxis("Horizontal");
        // float verticalInput = Input.GetAxis("Vertical");
        // return new Vector2(horizontalInput, verticalInput).normalized;
        // }

        // private void FixedUpdate() {
        // Calculate the movement direction based on input and the player's rotation
        // directionToMoveTo = new Vector3(controllerHandler.HorizontalMovementInput(), 0, controllerHandler.VerticalMovementInput());
        // directionToMoveTo = directionToMoveTo.normalized;

        // Move the character based on the movement direction and speed
        // characterController.Move(directionToMoveTo * (Maths.PositiveValue(globals.DefaultMovementSpeed) * Time.fixedDeltaTime));
        // }
    }
}
