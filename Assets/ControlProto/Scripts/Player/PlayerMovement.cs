using Cinemachine;
using ControlProto.Util;
using ControlProto.Util.Gravity;
using ControlProto.Util.PlayerInputSystem;
using ControlProto.Util.PlayerInputSystem.New;
using ControlProto.Util.PlayerRotation;
using ControlProto.Util.ThreeDimensionalMovement;
using ControlProto.Util.TwoDimensionalGroundPlaneMovement;
using UnityEngine;
using UnityEngine.InputSystem;

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
        [SerializeField] private float cameraOffsetFromPlayerCeiling;

        private const float MinPitch = -90;
        private const float MaxPitch = 90;

        private CharacterController controller;
        private DefaultInputActions defaultInputActions;
        private CameraRotation cameraRotation;
        private PlayerRotation playerRotation;
        private ThreeDimensionalMovement threeDimensionalMovement;
        private Camera playerCamera;
        private CinemachineBrain cinemachineBrain;
        private CinemachineVirtualCamera virtualCamera;

        private void Awake() {
            defaultInputActions = new DefaultInputActions();
            IPlayerInputSystem inputSystem = new NewPlayerInputSystem(defaultInputActions);

            InitializeController();
            InitializeCameras();

            CursorManager cursorManager = new CursorManager();
            PitchBounds pitchBounds = new PitchBounds(MinPitch, MaxPitch);
            MouseSensitivities mouseSensitivities = new MouseSensitivities(horizontalMouseSensitivity, verticalMouseSensitivity);
            Speeds speeds = new Speeds(crouchMovementSpeed, walkMovementSpeed, sprintMovementSpeed);
            GravityConstants gravityConstants = new GravityConstants(gravity, defaultVerticalVelocity, floatTolerance);
            SpeedManager speedManager = new SpeedManager(inputSystem, speeds);
            GravityManager gravityManager = new GravityManager(gravityConstants, transform);
            cameraRotation = new CameraRotation(inputSystem, mouseSensitivities, pitchBounds);
            playerRotation = new PlayerRotation(inputSystem, mouseSensitivities);
            TwoDimensionalMovement twoDimensionalMovement = new TwoDimensionalMovement(inputSystem, speedManager);
            threeDimensionalMovement = new ThreeDimensionalMovement(twoDimensionalMovement, gravityManager);
        }

        private void Update() {
            RotateCamera();
            RotatePlayer();
            MovePlayer();
        }

        private void RotateCamera() {
            virtualCamera.transform.localRotation = cameraRotation.Value();
        }

        private void RotatePlayer() {
            transform.rotation = playerRotation.Value();
        }

        private void MovePlayer() {
            Vector3 movementVector = threeDimensionalMovement.Value(transform);
            if (movementVector != Vector3.zero) {
                // Debug.Log($"Moving player from: {transform.position} to: {moveVector}");
                controller.Move(movementVector * Time.deltaTime);
            }
        }

        private void InitializeController() {
            // Offset the character mesh so that it is slightly above the character controller. This prevents the player from floating above the ground
            controller = gameObject.AddComponent<CharacterController>();
            controller.center += new Vector3(0, controller.skinWidth, 0);
        }

        /**
         * Requires a controller to be setup first
         */
        private void InitializeCameras() {
            GameObject cameraObject = new GameObject("CinemachineVirtualCamera");
            cameraObject.transform.SetParent(transform);
            cameraObject.transform.localPosition = new Vector3(0, (controller.height / 2) - cameraOffsetFromPlayerCeiling, 0);
            virtualCamera = cameraObject.AddComponent<CinemachineVirtualCamera>();

            GameObject cameraBrainObject = new GameObject("CameraBrain");
            cameraBrainObject.transform.SetParent(transform);
            playerCamera = cameraBrainObject.AddComponent<Camera>();
            cinemachineBrain = cameraBrainObject.AddComponent<CinemachineBrain>();
        }

        //
        // private void OnDisable() {
        //     defaultInputActions.Disable();
        //     foreach (InputAction inputAction in inputActions) {
        //         inputAction.Disable();
        //     }
        // }
        //
        // private void OnDestroy() {
        //     defaultInputActions.Dispose();
        //     foreach (InputAction inputAction in inputActions) {
        //         inputAction.Dispose();
        //     }
        // }
    }
}
