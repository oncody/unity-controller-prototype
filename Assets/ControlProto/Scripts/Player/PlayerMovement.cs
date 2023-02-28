using Cinemachine;
using ControlProto.Util;
using ControlProto.Util.Gravity;
using ControlProto.Util.PlayerController;
using ControlProto.Util.PlayerController.FirstPerson;
using ControlProto.Util.PlayerInputSystem;
using ControlProto.Util.PlayerInputSystem.New;
using ControlProto.Util.PlayerRotation;
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

        private DefaultInputActions defaultInputActions;
        private PlayerController firstPersonController;

        private void Awake() {
            defaultInputActions = new DefaultInputActions();
            IPlayerInputSystem inputSystem = new NewPlayerInputSystem(defaultInputActions);
            CharacterController controller = InitializeController();
            CinemachineVirtualCamera virtualCamera = InitializeCameras(controller);
            CursorManager cursorManager = new CursorManager();
            PitchBounds pitchBounds = new PitchBounds(MinPitch, MaxPitch);
            MouseSensitivities mouseSensitivities = new MouseSensitivities(horizontalMouseSensitivity, verticalMouseSensitivity);
            Speeds speeds = new Speeds(crouchMovementSpeed, walkMovementSpeed, sprintMovementSpeed);
            GravityConstants gravityConstants = new GravityConstants(gravity, defaultVerticalVelocity, floatTolerance);
            SpeedManager speedManager = new SpeedManager(inputSystem, speeds);
            GravityManager gravityManager = new GravityManager(gravityConstants, transform);
            firstPersonController = new FirstPersonController(inputSystem, controller, virtualCamera.transform, transform, gravityManager, speedManager, mouseSensitivities, pitchBounds);
        }

        private void Update() {
            firstPersonController.Update();
        }

        private CharacterController InitializeController() {
            // Offset the character mesh so that it is slightly above the character controller. This prevents the player from floating above the ground
            CharacterController controller = gameObject.AddComponent<CharacterController>();
            controller.center += new Vector3(0, controller.skinWidth, 0);
            return controller;
        }

        private CinemachineVirtualCamera InitializeCameras(CharacterController controller) {
            GameObject cameraObject = new GameObject("CinemachineVirtualCamera");
            cameraObject.transform.SetParent(transform);
            cameraObject.transform.localPosition = new Vector3(0, (controller.height / 2) - cameraOffsetFromPlayerCeiling, 0);
            CinemachineVirtualCamera virtualCamera = cameraObject.AddComponent<CinemachineVirtualCamera>();

            GameObject cameraBrainObject = new GameObject("CameraBrain");
            cameraBrainObject.transform.SetParent(transform);
            cameraBrainObject.AddComponent<Camera>();
            cameraBrainObject.AddComponent<CinemachineBrain>();
            return virtualCamera;
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
