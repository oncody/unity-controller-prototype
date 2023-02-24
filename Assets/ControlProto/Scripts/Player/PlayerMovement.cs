using System;
using ControlProto.Scripts.Global;
using ControlProto.Util;
using ControlProto.Util.Input.Controller;
using ControlProto.Util.RayCast;
using ControlProto.Util.Rotation;
using Unity.VisualScripting;
using UnityEngine;

namespace ControlProto.Scripts.Player {
    public class PlayerMovement : MonoBehaviour {
        [SerializeField] private Globals globals;
        [SerializeField] private CharacterController characterController;

        private readonly ControllerHandler controllerHandler = new();
        private GravityController gravityController;

        private float defaultMovementSpeed;
        private float groundCheckDistance;

        private void Start() {
            defaultMovementSpeed = globals.DefaultMovementSpeed;
            groundCheckDistance = globals.GroundCheckDistance;
            gravityController = new GravityController(globals.Gravity, globals.JumpHeight);
        }

        private void Update() {
            MovePlayer();
        }

        private void MovePlayer() {
            Vector3 movementVector = VerticalMoveVector() + HorizontalMovementVector();
            if (movementVector != Vector3.zero) {
                characterController.Move(movementVector * Time.deltaTime);
            }
        }

        private Vector3 VerticalMoveVector() {
            RayCastReturn groundedSphere = CheckIfGroundedWithSphereCast();
            RayCastReturn groundedRay = CheckIfGroundedWithRayCast();

            Debug.Log($"groundedRay: {groundedRay.EncounteredObject}");
            Debug.Log($"groundedSphere: {groundedSphere.EncounteredObject}");

            // todo: this value might need to be -2 sometimes to force the player to the ground when they're hovering right above it
            if (groundedSphere.EncounteredObject && Input.GetButtonDown("Jump")) {
                gravityController.ApplyJump();
            }

            gravityController.MoveForwardInTime(groundedSphere.EncounteredObject, Time.deltaTime);
            return gravityController.MoveVector();
        }

        private Vector3 HorizontalMovementVector() {
            Vector3 horizontalMovementVector = Vector3.zero;
            Vector3 keyboardMovementVector = new Vector3(controllerHandler.HorizontalKeyboardMovement(), 0, controllerHandler.VerticalKeyboardMovement());
            if (keyboardMovementVector != Vector3.zero) {
                Vector3 worldMoveDirection = transform.TransformDirection(keyboardMovementVector);
                horizontalMovementVector = worldMoveDirection.normalized * defaultMovementSpeed;
            }

            return horizontalMovementVector;
        }

        private RayCastReturn CheckIfGroundedWithSphereCast() {
            // Start the raycast at the center point of our character
            // Set the max ray distance to be slightly larger than half of the character's height
            float rayDistance = (characterController.height / 2.0f) + Maths.PositiveValue(groundCheckDistance);
            RayCastInput rayCastInput = new RayCastInput(transform.position, Vector3.down, rayDistance);
            SphereCastInput sphereCastInput = new SphereCastInput(rayCastInput, characterController.radius);

            return Raycasts.SphereCast(sphereCastInput, Physics.AllLayers, QueryTriggerInteraction.Ignore);
        }

        private RayCastReturn CheckIfGroundedWithRayCast() {
            // Start the raycast at the center point of our character
            // Set the max ray distance to be slightly larger than half of the character's height
            float rayDistance = (characterController.height / 2.0f) + Maths.PositiveValue(groundCheckDistance);
            RayCastInput rayCastInput = new RayCastInput(transform.position, Vector3.down, rayDistance);

            return Raycasts.RayCast(rayCastInput, Physics.AllLayers, QueryTriggerInteraction.Ignore);
        }
    }
}
