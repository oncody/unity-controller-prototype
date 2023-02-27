using ControlProto.Scripts.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ControlProto.Util {
    public class TwoDimensionMovement {
        private readonly float crouchMovementSpeed;
        private readonly float walkMovementSpeed;
        private readonly float sprintMovementSpeed;
        private readonly MoveInput moveInput;
        private readonly InputAction crouchAction;
        private readonly InputAction sprintAction;

        private GroundSpeed groundSpeed = GroundSpeed.Walking;
        private PlayerState playerState = PlayerState.Idle;
        private float groundSpeedValue;

        private bool isCrouchButtonHeldDown;
        private bool isSprintButtonHeldDown;

        public TwoDimensionMovement(DefaultInputActions defaultInputActions, float crouchMovementSpeed, float walkMovementSpeed, float sprintMovementSpeed) {
            moveInput = new MoveInput(defaultInputActions);
            this.crouchMovementSpeed = crouchMovementSpeed;
            this.walkMovementSpeed = walkMovementSpeed;
            this.sprintMovementSpeed = sprintMovementSpeed;
            groundSpeedValue = walkMovementSpeed;

            crouchAction = new InputAction("Crouch", InputActionType.Value, "<Keyboard>/leftCtrl");
            crouchAction.started += CrouchStartedCallback;
            crouchAction.canceled += CrouchCanceledCallback;
            crouchAction.Enable();

            sprintAction = new InputAction("Sprint", InputActionType.Value, "<Keyboard>/leftShift");
            sprintAction.started += SprintStartedCallback;
            sprintAction.canceled += SprintCanceledCallback;
            sprintAction.Enable();
        }

        public Vector3 CalculateTwoDimensionalMovement(Transform transform) {
            if (moveInput.Value == Vector2.zero) {
                return Vector3.zero;
            }

            Vector3 localMoveDirection = new Vector3(moveInput.Value.x, 0, moveInput.Value.y);
            Vector3 worldMoveDirection = transform.TransformDirection(localMoveDirection);
            return worldMoveDirection.normalized * groundSpeedValue;
        }

        private void CrouchStartedCallback(InputAction.CallbackContext context) {
            isCrouchButtonHeldDown = true;
            groundSpeed = GroundSpeed.Crouching;
            groundSpeedValue = crouchMovementSpeed;
        }

        private void CrouchCanceledCallback(InputAction.CallbackContext context) {
            isCrouchButtonHeldDown = false;
            groundSpeed = isSprintButtonHeldDown ? GroundSpeed.Sprinting : GroundSpeed.Walking;
            groundSpeedValue = isSprintButtonHeldDown ? sprintMovementSpeed : walkMovementSpeed;
        }

        private void SprintStartedCallback(InputAction.CallbackContext context) {
            isSprintButtonHeldDown = true;
            groundSpeed = isCrouchButtonHeldDown ? GroundSpeed.Crouching : GroundSpeed.Sprinting;
            groundSpeedValue = isCrouchButtonHeldDown ? crouchMovementSpeed : sprintMovementSpeed;
        }

        private void SprintCanceledCallback(InputAction.CallbackContext context) {
            isSprintButtonHeldDown = false;
            groundSpeed = isCrouchButtonHeldDown ? GroundSpeed.Crouching : GroundSpeed.Walking;
            groundSpeedValue = isCrouchButtonHeldDown ? crouchMovementSpeed : walkMovementSpeed;
        }
    }
}
