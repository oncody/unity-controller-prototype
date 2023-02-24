using System;
using ControlProto.Scripts.Global;
using ControlProto.Util;
using ControlProto.Util.Input.Controller;
using ControlProto.Util.Input.Keyboard;
using ControlProto.Util.RayCast;
using ControlProto.Util.Rotation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ControlProto.Scripts.Player {
    public class PlayerMovement : MonoBehaviour {
        [SerializeField] private Globals globals;
        [SerializeField] private CharacterController characterController;

        private ControllerHandler controllerHandler;
        private KeyboardHandler keyboardHandler;
        private GravityController gravityController;
        private SpeedController speedController;

        private float groundCheckDistance;

        private void Start() {
            Mouse mouse = InputSystem.GetDevice<Mouse>();
            Keyboard keyboard = InputSystem.GetDevice<Keyboard>();

            controllerHandler = new ControllerHandler(mouse, keyboard);
            keyboardHandler = new KeyboardHandler();

            groundCheckDistance = Maths.PositiveValue(globals.GroundCheckDistance);
            gravityController = new GravityController(globals.Gravity, globals.JumpHeight);
            speedController = new SpeedController(globals.CrouchMovementSpeed, globals.WalkMovementSpeed, globals.SprintMovementSpeed);
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
            Vector3 moveVector = Vector3.zero;
            Vector3 playerPosition = transform.position;

            // Start the raycast at the center point of our character
            // Set the max ray distance to be slightly larger than half of the character's height
            float rayDistance = (characterController.height / 2.0f) + groundCheckDistance;
            RayCastInput rayCastInput = new RayCastInput(playerPosition, Vector3.down, rayDistance);
            SphereCastInput sphereCastInput = new SphereCastInput(rayCastInput, characterController.radius);

            RayCastReturn groundedSphere = Raycasts.SphereCast(sphereCastInput, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            RayCastReturn groundedRay = Raycasts.RayCast(rayCastInput, Physics.AllLayers, QueryTriggerInteraction.Ignore);

            // apply move vector from sphere to point
            if (groundedSphere.EncounteredObject && !groundedRay.EncounteredObject) {
                // we are near an edge. gently push the person fall off
                moveVector = playerPosition - groundedSphere.Hit.point;

                // remove vertical from this
                moveVector = new Vector3(moveVector.x, 0, moveVector.z);
            }

            if (groundedSphere.EncounteredObject && UnityEngine.Input.GetKeyDown(KeyCode.Space)) {
                gravityController.ApplyJump();
            }

            gravityController.MoveForwardInTime(groundedSphere.EncounteredObject, Time.deltaTime);
            moveVector += gravityController.MoveVector();
            return moveVector;
        }

        private Vector3 HorizontalMovementVector() {
            Vector3 horizontalMovementVector = Vector3.zero;
            Vector3 keyboardMovementVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            if (keyboardMovementVector != Vector3.zero) {
                Vector3 worldMoveDirection = transform.TransformDirection(keyboardMovementVector);
                horizontalMovementVector = worldMoveDirection.normalized * speedController.MovementSpeed();
            }

            return horizontalMovementVector;
        }
    }
}
