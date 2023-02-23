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
        private bool grounded;

        // Pitch is up and down rotation of an object. Like tilting your head forward and backward
        private float pitch = 0.0f;

        // Yaw refers to left and right rotation of an object. For example, turning your head and looking left or right
        private float yaw = 0.0f;


        private void Update() {
            UpdateGrounded();
            UpdateCameraRotation();
            UpdateVerticalVelocity();
            Vector3 horizontalMovement = CalculateHorizontalMovement() * (globals.DefaultMovementSpeed);
            Vector3 verticalMovement = new Vector3(0, verticalVelocity, 0);
            Vector3 combinedMovement = horizontalMovement + verticalMovement;
            characterController.Move(combinedMovement * Time.deltaTime);
        }

        private void FixedUpdate() {
            //todo: try moving character . move code here
        }

        private void LateUpdate() {
            // todo: move this out of update and into lateupdate
            // UpdateCameraRotation();
        }

        private void UpdateGrounded() {
            // Set the distance of the ray to be slightly larger than the character's height
            float rayDistance = (characterController.height / 2.0f) + Maths.PositiveValue(globals.GroundCheckDistance);
            grounded = Physics.SphereCast(transform.position, characterController.radius, Vector3.down, out _, rayDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore);
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
            if (characterController.isGrounded) {
                if (verticalVelocity < 0) {
                    verticalVelocity = -2f;
                }

                if (Input.GetButtonDown("Jump") && characterController.isGrounded) {
                    verticalVelocity = Mathf.Sqrt(Maths.PositiveValue(globals.JumpHeight) * 2f * Maths.PositiveValue(globals.Gravity));
                }
            }
            else {
                verticalVelocity += Maths.NegativeValue(globals.Gravity) * Time.deltaTime;
            }

            Debug.Log($"verticalVelocity: {verticalVelocity}");
        }
    }
}
