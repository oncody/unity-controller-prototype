using System;
using ControlProto.Scripts.Global;
using ControlProto.Util;
using ControlProto.Util.Input.Controller;
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
            UpdateVerticalVelocity();
            MovePlayer();
        }

        private void UpdateVerticalVelocity() {
            // bool groundedSphere = CheckIfGroundedWithSphereCast();
            bool groundedRay = CheckIfGroundedWithRayCast();

            Debug.Log($"groundedRay: {groundedRay}");

            // todo: this value might need to be -2 sometimes to force the player to the ground when they're hovering right above it
            if (groundedRay && Input.GetButtonDown("Jump")) {
                gravityController.ApplyJump();
            }

            gravityController.MoveForwardInTime(groundedRay, Time.deltaTime);
        }

        private bool CheckIfGroundedWithSphereCast() {
            // Start the raycast at the center point of our character
            // Set the max ray distance to be slightly larger than half of the character's height
            Vector3 sphereOrigin = transform.position;
            float sphereRadius = characterController.radius;
            Vector3 sphereDirection = Vector3.down;
            float sphereMaxDistance = (characterController.height / 2.0f) + Maths.PositiveValue(groundCheckDistance);
            int layerMask = Physics.AllLayers;
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore;
            return Physics.SphereCast(sphereOrigin, sphereRadius, sphereDirection, out _, sphereMaxDistance, layerMask, queryTriggerInteraction);
        }

        private bool CheckIfGroundedWithRayCast() {
            // Start the raycast at the center point of our character
            // Set the max ray distance to be slightly larger than half of the character's height
            RaycastHit raycastHit;
            Vector3 rayOrigin = transform.position;
            Vector3 sphereDirection = Vector3.down;
            float sphereMaxDistance = (characterController.height / 2.0f) + Maths.PositiveValue(groundCheckDistance);
            int layerMask = Physics.AllLayers;
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore;

            return Physics.Raycast(rayOrigin, sphereDirection, out raycastHit, sphereMaxDistance, layerMask, queryTriggerInteraction);
        }

        private void MovePlayer() {
            Vector3 verticalMovementVector = new Vector3(0, gravityController.GetVelocity(), 0);
            Vector3 combinedMovementVector = CalculateHorizontalMovementVector() + verticalMovementVector;

            if (combinedMovementVector != Vector3.zero) {
                characterController.Move(combinedMovementVector * Time.deltaTime);
            }
        }

        private Vector3 CalculateHorizontalMovementVector() {
            Vector3 horizontalMovementVector = Vector3.zero;
            Vector3 keyboardMovementVector = new Vector3(controllerHandler.HorizontalKeyboardMovement(), 0, controllerHandler.VerticalKeyboardMovement());
            if (keyboardMovementVector != Vector3.zero) {
                Vector3 worldMoveDirection = transform.TransformDirection(keyboardMovementVector);
                horizontalMovementVector = worldMoveDirection.normalized * defaultMovementSpeed;
            }

            return horizontalMovementVector;
        }
    }
}
