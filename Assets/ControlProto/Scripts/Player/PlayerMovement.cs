using System;
using System.Collections;
using System.Collections.Generic;
using ControlProto.Scripts.Global;
using ControlProto.Util;
using ControlProto.Util.Input.Controller;
using UnityEngine;

namespace ControlProto.Scripts.Player {
    public class PlayerMovement : MonoBehaviour {
        [SerializeField] private CharacterController controller;
        [SerializeField] private Globals globals;
        private ControllerHandler controllerHandler;
        private Vector3 velocity;
        private bool isGrounded;
        private Vector3 previousPosition;
        private float previousTime;
        private float maxRecordedMovementSpeed = 0f;

        void Start() {
            controllerHandler = new ControllerHandler();

            previousPosition = transform.position;
            previousTime = Time.time;
        }

        void Update() {
            Transform currentTransform = transform;

            // Store the current position and time for the next frame
            Vector3 position = currentTransform.position;

            // Get the collider component
            // TODO: CODY change this to bind to a collider
            Collider currentCollider = GetComponent<Collider>();
            Bounds bounds = currentCollider.bounds;

            // Calculate the distance travelled since the last frame
            Vector3 distanceTravelled = position - previousPosition;

            // Calculate the time elapsed since the last frame
            float deltaTime = Time.time - previousTime;

            // Calculate the speed as the distance divided by the time
            float thisSpeed = distanceTravelled.magnitude / deltaTime;

            if (thisSpeed > maxRecordedMovementSpeed) {
                maxRecordedMovementSpeed = thisSpeed;
            }

            previousPosition = position;
            previousTime = Time.time;

            Debug.Log("Speed: " + thisSpeed);
            Debug.Log("maxSpeed: " + maxRecordedMovementSpeed);

            float topPosition = position.y + bounds.extents.y;
            float bottomPosition = position.y - bounds.extents.y;
            float characterHeight = topPosition - bottomPosition;

            // Debug.Log($"topPosition: {topPosition}");
            // Debug.Log($"bottomPosition: {bottomPosition}");
            Debug.Log($"characterHeight: {characterHeight}");

            RaycastHit hit;
            // Set the distance of the ray to be slightly larger than the character's height
            float rayDistance = (characterHeight / 2.0f) + Maths.PositiveValue(globals.GroundCheckDistance);
            isGrounded = Physics.Raycast(position, Vector3.down, out hit, rayDistance, Physics.AllLayers);

            Debug.Log($"isGrounded: {isGrounded}");

            if (isGrounded && velocity.y < 0) {
                velocity.y = 0;
            }

            float horizontalMovementValue = controllerHandler.HorizontalMovementInput();
            float verticalMovementValue = controllerHandler.VerticalMovementInput();

            Vector3 directionToMoveTo = currentTransform.right * horizontalMovementValue + currentTransform.forward * verticalMovementValue;
            controller.Move(directionToMoveTo * (Maths.PositiveValue(globals.DefaultMovementSpeed) * Time.deltaTime));

            velocity.y -= globals.Gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }

        public bool IsGrounded() {
            return isGrounded;
        }
    }
}
