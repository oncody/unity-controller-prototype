using Cinemachine;
using ControlProto.Util;
using ControlProto.Util.Gravity;
using ControlProto.Util.PlayerController;
using ControlProto.Util.PlayerController.FirstPerson;
using ControlProto.Util.PlayerController.ThirdPerson;
using ControlProto.Util.PlayerInputSystem;
using ControlProto.Util.PlayerInputSystem.New;
using ControlProto.Util.TwoDimensionalGroundPlaneMovement;
using ControlProto.Util.Unity;
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
        private PlayerController thirdPersonController;

        private void Awake() {
            defaultInputActions = new DefaultInputActions();
            IPlayerInputSystem inputSystem = new NewPlayerInputSystem(defaultInputActions);
            CharacterController controller = InitializeController();
            Transform sceneCamera = InitializeCamera();
            PlayerObjects playerObjects = new PlayerObjects(controller, transform, sceneCamera);

            CursorManager cursorManager = new CursorManager();
            PlayerLookConstants lookConstants = new PlayerLookConstants(horizontalMouseSensitivity, verticalMouseSensitivity, MinPitch, MaxPitch);
            Speeds speeds = new Speeds(crouchMovementSpeed, walkMovementSpeed, sprintMovementSpeed);
            GravityConstants gravityConstants = new GravityConstants(gravity, defaultVerticalVelocity, floatTolerance);
            SpeedManager speedManager = new SpeedManager(inputSystem, speeds);
            GravityManager gravityManager = new GravityManager(gravityConstants, transform);
            // firstPersonController = new FirstPersonController(inputSystem, playerObjects, gravityManager, speedManager, lookConstants, cameraOffsetFromPlayerCeiling);
            thirdPersonController = new ThirdPersonController(inputSystem, playerObjects, gravityManager, speedManager, lookConstants);

            // disabling rigid body physics as we are using a character controller
            Physics.autoSimulation = false;
        }

        private void Update() {
            // firstPersonController.Update();
            thirdPersonController.Update();
        }

        private CharacterController InitializeController() {
            // Offset the character mesh so that it is slightly above the character controller. This prevents the player from floating above the ground
            CharacterController controller = gameObject.AddComponent<CharacterController>();
            controller.center += new Vector3(0, controller.skinWidth, 0);
            return controller;
        }

        private Transform InitializeCamera() {
            GameObject cameraBrainObject = new GameObject("CameraBrain");
            cameraBrainObject.transform.SetParent(transform);
            cameraBrainObject.AddComponent<CinemachineBrain>();
            Camera sceneCamera = cameraBrainObject.AddComponent<Camera>();
            return sceneCamera.transform;
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
