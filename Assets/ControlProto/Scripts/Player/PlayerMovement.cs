using Cinemachine;
using ControlProto.Util;
using ControlProto.Util.Gravity;
using ControlProto.Util.PlayerController;
using ControlProto.Util.PlayerController.FirstPerson;
using ControlProto.Util.PlayerController.ThirdPerson;
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
        private PlayerController thirdPersonController;

        private void Awake() {
            defaultInputActions = new DefaultInputActions();
            IPlayerInputSystem inputSystem = new NewPlayerInputSystem(defaultInputActions);
            CharacterController controller = InitializeController();
            CinemachineFreeLook virtualCamera = InitializeFreeLookCamera(controller);
            Camera sceneCamera = InitializePlayerCamera();
            CursorManager cursorManager = new CursorManager();
            PitchBounds pitchBounds = new PitchBounds(MinPitch, MaxPitch);
            MouseSensitivities mouseSensitivities = new MouseSensitivities(horizontalMouseSensitivity, verticalMouseSensitivity);
            Speeds speeds = new Speeds(crouchMovementSpeed, walkMovementSpeed, sprintMovementSpeed);
            GravityConstants gravityConstants = new GravityConstants(gravity, defaultVerticalVelocity, floatTolerance);
            SpeedManager speedManager = new SpeedManager(inputSystem, speeds);
            GravityManager gravityManager = new GravityManager(gravityConstants, transform);
            firstPersonController = new FirstPersonController(inputSystem, controller, transform, virtualCamera.transform, gravityManager, speedManager, mouseSensitivities, pitchBounds);
            thirdPersonController = new ThirdPersonController(inputSystem, controller, transform, virtualCamera.transform, sceneCamera.transform, gravityManager, speedManager, mouseSensitivities, pitchBounds);
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

        private CinemachineFreeLook InitializeFreeLookCamera(CharacterController controller) {
            GameObject cameraObject = new GameObject("ThirdPersonCamera");
            cameraObject.transform.SetParent(transform);
            cameraObject.transform.localPosition = new Vector3(0, (controller.height / 2) - cameraOffsetFromPlayerCeiling, 0);
            CinemachineFreeLook freeLookCamera = cameraObject.AddComponent<CinemachineFreeLook>();
            freeLookCamera.m_Follow = transform;
            freeLookCamera.m_LookAt = transform;
            freeLookCamera.m_BindingMode = CinemachineTransposer.BindingMode.WorldSpace;
            freeLookCamera.m_YAxis.m_InvertInput = true;
            freeLookCamera.m_XAxis.m_InvertInput = false;
            freeLookCamera.m_Orbits[0].m_Height = 14;
            freeLookCamera.m_Orbits[0].m_Radius = 12;
            freeLookCamera.m_Orbits[1].m_Height = 5;
            freeLookCamera.m_Orbits[1].m_Radius = 18;
            freeLookCamera.m_Orbits[2].m_Height = 1.5f;
            freeLookCamera.m_Orbits[2].m_Radius = 12;
            return freeLookCamera;
        }

        private Camera InitializePlayerCamera() {
            GameObject cameraBrainObject = new GameObject("CameraBrain");
            cameraBrainObject.transform.SetParent(transform);
            cameraBrainObject.AddComponent<CinemachineBrain>();
            return cameraBrainObject.AddComponent<Camera>();
        }

        // private CinemachineVirtualCamera InitializeCameras(CharacterController controller) {
        //     GameObject cameraObject = new GameObject("CinemachineVirtualCamera");
        //     cameraObject.transform.SetParent(transform);
        //     cameraObject.transform.localPosition = new Vector3(0, (controller.height / 2) - cameraOffsetFromPlayerCeiling, 0);
        //     CinemachineVirtualCamera virtualCamera = cameraObject.AddComponent<CinemachineVirtualCamera>();
        //
        //     GameObject cameraBrainObject = new GameObject("CameraBrain");
        //     cameraBrainObject.transform.SetParent(transform);
        //     cameraBrainObject.AddComponent<Camera>();
        //     cameraBrainObject.AddComponent<CinemachineBrain>();
        //     return virtualCamera;
        // }

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
