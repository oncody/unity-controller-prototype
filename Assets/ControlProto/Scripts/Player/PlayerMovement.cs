using System;
using System.Collections.Generic;
using Cinemachine;
using ControlProto.Util;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace ControlProto.Scripts.Player {
    public class PlayerMovement : MonoBehaviour {
        [SerializeField] private float verticalMouseSensitivity;
        [SerializeField] private float horizontalMouseSensitivity;
        [SerializeField] private float crouchMovementSpeed;
        [SerializeField] private float walkMovementSpeed;
        [SerializeField] private float sprintMovementSpeed;
        [SerializeField] private float gravity;
        [SerializeField] private float floatTolerance;
        [SerializeField] private float defaultVerticalVelocity;
        [SerializeField] private float cameralOffsetFromPlayerCeiling;

        private CharacterController playerController;

        // private Transform playerCamera;
        private CinemachineOrbitalTransposer thirdPersonCinemachineOrbitalTransposer;
        private CinemachineVirtualCamera thirdPersonCinemachineCamera;
        private CinemachineCollider thirdPersonCinemachineCameraCollider;
        private CinemachineBrain thirdPersonCinemachineCameraBrain;
        private Camera thirdPersonSimpleCamera;

        private const float MaxPitch = 90;
        private const float MinPitch = -90;

        private readonly List<InputAction> inputActions = new();
        private DefaultInputActions defaultInputActions;

        private GroundSpeed groundSpeed = GroundSpeed.Walking;
        private PlayerState playerState = PlayerState.Idle;

        private Vector3 lastPosition;
        private Vector2 inputMoveVector;
        private Vector2 inputLookVector;

        private float pitch; // up-down rotation around x-axis
        private float yaw; // left-right rotation around y-axis
        private float groundSpeedValue;
        private float verticalVelocity;

        private bool isCrouchButtonHeldDown;
        private bool isSprintButtonHeldDown;
        private bool startedFallingVertically;
        private bool finishedFallingVertically = true;

        private void Awake() {
            verticalVelocity = defaultVerticalVelocity;
            groundSpeedValue = walkMovementSpeed;

            CreateController();
            CreateThirdPersonCamera();
            LockCursor();
            BindInput();
        }

        private void Start() {
        }

        private void Update() {
            UpdateFallingCheck();
            RotateThirdPersonPlayerAndCamera();
            MovePlayer();
        }

        private void ExitFocusCallback(InputAction.CallbackContext context) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void CrouchStartedCallback(InputAction.CallbackContext context) {
            isCrouchButtonHeldDown = true;
            UpdateGroundSpeed();
        }

        private void CrouchCanceledCallback(InputAction.CallbackContext context) {
            isCrouchButtonHeldDown = false;
            UpdateGroundSpeed();
        }

        private void SprintStartedCallback(InputAction.CallbackContext context) {
            isSprintButtonHeldDown = true;
            UpdateGroundSpeed();
        }

        private void SprintCanceledCallback(InputAction.CallbackContext context) {
            isSprintButtonHeldDown = false;
            UpdateGroundSpeed();
        }

        private void OnEnable() {
            defaultInputActions.Enable();
            foreach (InputAction inputAction in inputActions) {
                inputAction.Enable();
            }
        }

        private void OnDisable() {
            defaultInputActions.Disable();
            foreach (InputAction inputAction in inputActions) {
                inputAction.Disable();
            }
        }

        private void OnDestroy() {
            defaultInputActions.Dispose();
            foreach (InputAction inputAction in inputActions) {
                inputAction.Dispose();
            }
        }

        private void LockCursor() {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void CreateController() {
            // Offset the character mesh so that it is slightly above the character controller. This prevents the player from floating
            playerController = gameObject.AddComponent<CharacterController>();
            playerController.center += new Vector3(0, playerController.skinWidth, 0);
        }

        // private void CreateFirstPersonCamera() {
        //     GameObject firstPersonCamera = new GameObject("FirstPersonCamera");
        //     playerCamera = firstPersonCamera.transform;
        //     playerCamera.SetParent(transform);
        //     playerCamera.localPosition = new Vector3(0, (playerController.height / 2) - cameralOffsetFromPlayerCeiling, 0);
        //     CinemachineVirtualCamera cinemachineCameraComponent = firstPersonCamera.AddComponent<CinemachineVirtualCamera>();
        //
        //     GameObject cameraBrainObject = new GameObject("CameraBrain");
        //     cameraBrainObject.transform.SetParent(transform);
        //     Camera cameraComponent = cameraBrainObject.AddComponent<Camera>();
        //     CinemachineBrain brainComponent = cameraBrainObject.AddComponent<CinemachineBrain>();
        // }

        private void CreateThirdPersonCamera() {
            GameObject cameraObject = new("CinemachineCamera");
            Transform playerCamera = cameraObject.transform;
            playerCamera.SetParent(transform);
            playerCamera.localPosition = new Vector3(0, playerController.height + 2, -10);
            // thirdPersonCinemachineCamera = cameraObject.AddComponent<CinemachineFreeLook>();
            thirdPersonCinemachineCamera = cameraObject.AddComponent<CinemachineVirtualCamera>();
            thirdPersonCinemachineCamera.Follow = transform;
            // thirdPersonCinemachineCamera.LookAt = transform;
            // thirdPersonCinemachineCamera.bind
            // thirdPersonCinemachineCamera.ai
            // thirdPersonCinemachineCamera.LookAt = transform;
            // thirdPersonCinemachineCamera.m_XAxis.m_InvertInput = false;
            // thirdPersonCinemachineCamera.m_YAxis.m_InvertInput = true;
            // thirdPersonCinemachineCamera.m_BindingMode = CinemachineTransposer.BindingMode.WorldSpace;

            // thirdPersonCinemachineCamera.AddCinemachineComponent<cin>()

            // top rig
            // thirdPersonCinemachineCamera.m_Orbits[0].m_Height = 14;
            // thirdPersonCinemachineCamera.m_Orbits[0].m_Radius = 12;

            // middle rig
            // thirdPersonCinemachineCamera.m_Orbits[1].m_Height = 5;
            // thirdPersonCinemachineCamera.m_Orbits[1].m_Radius = 18;

            // bottom rig
            // thirdPersonCinemachineCamera.m_Orbits[2].m_Height = -1;
            // thirdPersonCinemachineCamera.m_Orbits[2].m_Radius = 12;

            // todo: maybe this is how I should setup brain?? or the collidre??
            // thirdPersonCinemachineCamera.AddCinemachineComponent<CinemachineHardLockToTarget>();
            thirdPersonCinemachineOrbitalTransposer = thirdPersonCinemachineCamera.AddCinemachineComponent<CinemachineOrbitalTransposer>();
            thirdPersonCinemachineOrbitalTransposer.m_FollowOffset = new Vector3(0, 2f, -5f);
            // thirdPersonCinemachineOrbitalTransposer.m_BindingMode = CinemachineTransposer.BindingMode.LockToTarget;
            // thirdPersonCinemachineOrbitalTransposer.m_XAxis.angle
            // thirdPersonCinemachineOrbitalTransposer.m_XAxis.Value
            // thirdPersonCinemachineOrbitalTransposer.m_Heading.m_Definition

            thirdPersonCinemachineCameraCollider = cameraObject.AddComponent<CinemachineCollider>();
            thirdPersonCinemachineCameraCollider.m_Strategy = CinemachineCollider.ResolutionStrategy.PullCameraForward;
            thirdPersonCinemachineCameraCollider.m_CollideAgainst = 1 << LayerMask.NameToLayer("Ground");
            thirdPersonCinemachineCamera.AddExtension(thirdPersonCinemachineCameraCollider);
            // playerCameraCollider.m_IgnoreTag =
            // thirdPersonCamera.AddExtension(Ci);


            //todO:: maybe i dont need brain anymore??
            GameObject cameraBrain = new GameObject("CameraBrain");
            cameraBrain.transform.SetParent(transform);
            thirdPersonSimpleCamera = cameraBrain.AddComponent<Camera>();
            thirdPersonCinemachineCameraBrain = cameraBrain.AddComponent<CinemachineBrain>();
        }

        // private void RotatePlayerAndCamera() {
        //     yaw += inputLookVector.x * horizontalMouseSensitivity;
        //     pitch -= inputLookVector.y * verticalMouseSensitivity;
        //
        //     pitch = Mathf.Clamp(pitch, MinPitch, MaxPitch);
        //
        //     // Rotate camera up and down
        //     playerCamera.localRotation = Quaternion.Euler(pitch, 0, 0);
        //
        //     // Rotate player left and right
        //     transform.rotation = Quaternion.Euler(0, yaw, 0);
        // }

        private void RotateThirdPersonPlayerAndCamera() {
            // thirdPersonCamera.m_XAxis.Value += inputLookVector.x * horizontalMouseSensitivity;
            // thirdPersonCamera.m_YAxis.Value += inputLookVector.y * verticalMouseSensitivity;


            // Vector3 moveDir = Quaternion.Euler(0f, targetangle, 0f) * Vector3.forward;

            // pitch = Mathf.Clamp(pitch, MinPitch, MaxPitch);

            // Rotate camera up and down
            // playerCamera.localRotation = Quaternion.Euler(pitch, 0, 0);

            // Rotate player left and right
            // transform.rotation = Quaternion.Euler(0, yaw, 0);
        }

        private void MovePlayer() {
            // Vector3 moveVector = CalculateHorizontalMovement();

            // third person rotation

            // Vector3 inputMoveVector3 = new Vector3(inputMoveVector.x, 0, inputMoveVector.y).normalized * groundSpeedValue;

            // normalize converts the magnitude to 1 no matter what
            // return worldMoveDirection.normalized * groundSpeedValue;

            // thirdPersonCinemachineCamera.m_XAxis.Value += inputLookVector.x;
            // thirdPersonCinemachineCamera.Value += inputLookVector.x;

            // invert mouse
            // thirdPersonCinemachineCamera.m_YAxis.Value -= inputLookVector.y;
            // thirdPersonCinemachineCamera.m_YAxis.Value -= inputLookVector.y;


            // float horizontalRotation = thirdPersonCinemachineCamera.transform.rotation.eulerAngles.y;
            // float verticalRotation = thirdPersonCinemachineCamera.transform.rotation.eulerAngles.x;
            // Vector3 euler = thirdPersonCinemachineCamera.transform.rotation.eulerAngles;
            // euler.x -= inputLookVector.y * horizontalMouseSensitivity;
            // float orbitAmount = inputLookVector.x * 10f * Time.deltaTime;
            // thirdPersonCinemachineOrbitalTransposer.m_Heading.m_Definition.
            thirdPersonCinemachineOrbitalTransposer.m_XAxis.Value += inputMoveVector.x * Time.deltaTime;
            thirdPersonCinemachineOrbitalTransposer.m_YAxis.Value += inputMoveVector.x * Time.deltaTime;

            // thirdPersonCinemachineCamera.transform.rotation = Quaternion.Euler(euler);
            // thirdPersonCinemachineCamera.transform.position = transform.position - thirdPersonCinemachineCamera.transform.rotation * Vector3.forward; //* thirdPersonCinemachineCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>().r;


            // horizontalRotation += inputLookVector.x * horizontalMouseSensitivity;
            // verticalRotation -= inputLookVector.y * verticalMouseSensitivity;
            // verticalRotation = Mathf.Clamp(verticalRotation, MinPitch, MaxPitch);

            // Rotate camera up and down
            // TODO: CODY maybe it should be rotation instead of local
            // thirdPersonCinemachineCamera.transform.localRotation = Quaternion.Euler(pitch, yaw, 0);
            // thirdPersonCinemachineCamera.transform.rotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);

            Vector3 moveVector = Vector3.zero;
            if (inputMoveVector != Vector2.zero) {
                // float angle = Mathf.Atan2(inputMoveVector.x, inputMoveVector.y) * Mathf.Rad2Deg + thirdPersonSimpleCamera.transform.eulerAngles.y;
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                // transform.rotate =
                moveVector = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
                moveVector = moveVector.normalized * groundSpeedValue;
            }

            Vector3 verticalMoveVector = CalculateVerticalMovement();

            // if we have horizontal movement, then we might move them off a ledge close to the ground. add a small amount of gravity to pull them down in case.
            if (moveVector != Vector3.zero && verticalMoveVector == Vector3.zero) {
                verticalMoveVector = new Vector3(0, defaultVerticalVelocity, 0);
            }

            moveVector += verticalMoveVector;

            if (moveVector != Vector3.zero) {
                // Debug.Log($"Moving player from: {transform.position} to: {moveVector}");
                playerController.Move(moveVector * Time.deltaTime);
            }
        }

        private bool CurrentlyFalling() {
            return startedFallingVertically && !finishedFallingVertically;
        }

        private Vector3 CalculateHorizontalMovement() {
            if (inputMoveVector == Vector2.zero) {
                return Vector3.zero;
            }

            Vector3 inputMoveVector3 = new Vector3(inputMoveVector.x, 0, inputMoveVector.y);
            Vector3 worldMoveDirection = transform.TransformDirection(inputMoveVector3);

            // normalize converts the magnitude to 1 no matter what
            return worldMoveDirection.normalized * groundSpeedValue;
        }

        public Vector3 CalculateVerticalMovement() {
            if (!CurrentlyFalling()) {
                verticalVelocity = defaultVerticalVelocity;
                return Vector3.zero;
            }

            verticalVelocity -= gravity * Time.deltaTime;
            return new Vector3(0, verticalVelocity, 0);
        }

        // void LookMouseHorizontalCallback(InputAction.CallbackContext context) {
        //     // Update the camera's rotation based on the input values.
        //     thirdPersonCinemachineCamera.m_XAxis.Value += context.ReadValue<float>();
        // }
        //
        // void LookMouseVerticalCallback(InputAction.CallbackContext context) {
        //     // Update the camera's rotation based on the input values.
        //     thirdPersonCinemachineCamera.m_YAxis.Value += context.ReadValue<float>();
        // }

        void PlayerLookCallback(InputAction.CallbackContext context) {
            inputLookVector = context.ReadValue<Vector2>();
        }

        void PlayerLookCanceledCallback(InputAction.CallbackContext context) {
            inputLookVector = Vector2.zero;
        }

        void PlayerMovementCanceledCallback(InputAction.CallbackContext context) {
            inputMoveVector = Vector2.zero;
        }

        void PlayerMovementCallback(InputAction.CallbackContext context) {
            inputMoveVector = context.ReadValue<Vector2>();
        }

        private void UpdateGroundSpeed() {
            if (isCrouchButtonHeldDown) {
                groundSpeed = GroundSpeed.Crouching;
                groundSpeedValue = crouchMovementSpeed;
            }
            else if (isSprintButtonHeldDown) {
                groundSpeed = GroundSpeed.Sprinting;
                groundSpeedValue = sprintMovementSpeed;
            }
            else {
                groundSpeed = GroundSpeed.Walking;
                groundSpeedValue = walkMovementSpeed;
            }
        }

        private void UpdateFallingCheck() {
            float lastYPosition = lastPosition.y;
            float yPosition = transform.position.y;
            bool isFallingVertically = (Mathf.Abs(yPosition - lastYPosition) > floatTolerance) && (yPosition < lastYPosition);

            if (isFallingVertically && !startedFallingVertically) {
                startedFallingVertically = true;
                finishedFallingVertically = false;
            }

            if (!isFallingVertically && startedFallingVertically) {
                startedFallingVertically = false;
                finishedFallingVertically = true;
            }

            lastPosition = transform.position;
        }

        private void BindInput() {
            defaultInputActions = new DefaultInputActions();
            defaultInputActions.Player.Move.performed += PlayerMovementCallback;
            defaultInputActions.Player.Move.canceled += PlayerMovementCanceledCallback;

            defaultInputActions.Player.Look.performed += PlayerLookCallback;
            defaultInputActions.Player.Look.canceled += PlayerLookCanceledCallback;
            // Get the input actions for the X and Y axes.
            // thirdPersonCamera.m_XAxis.m_InputAxisName = mouseHorizontal.name; // i think you use name or value. not both. so lets use value
            // InputAction mouseHorizontal = new InputAction("Mouse Horizontal", InputActionType.Value, "<Mouse>/delta/x");
            // mouseHorizontal.performed += LookMouseHorizontalCallback;
            // inputActions.Add(mouseHorizontal);

            // InputAction mouseVertical = new InputAction("Mouse Vertical", InputActionType.Value, "<Mouse>/delta/y");
            // thirdPersonCamera.m_YAxis.m_InputAxisName = mouseVertical.name; // i think you use name or value. not both. so lets use value
            // mouseVertical.performed += LookMouseVerticalCallback;
            // inputActions.Add(mouseVertical);

            InputAction crouchAction = new InputAction("Crouch", InputActionType.Value, "<Keyboard>/leftCtrl");
            crouchAction.started += CrouchStartedCallback;
            crouchAction.canceled += CrouchCanceledCallback;
            inputActions.Add(crouchAction);

            InputAction sprintAction = new InputAction("Sprint", InputActionType.Value, "<Keyboard>/leftShift");
            sprintAction.started += SprintStartedCallback;
            sprintAction.canceled += SprintCanceledCallback;
            inputActions.Add(sprintAction);

            InputAction exitFocusAction = new InputAction("ExitFocus", InputActionType.Button, "<Keyboard>/escape");
            exitFocusAction.performed += ExitFocusCallback;
            inputActions.Add(exitFocusAction);

            defaultInputActions.Enable();
            foreach (InputAction inputAction in inputActions) {
                inputAction.Enable();
            }
        }
    }
}
