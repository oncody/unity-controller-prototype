using ControlProto.Scripts.Global;
using ControlProto.Util;
using ControlProto.Util.Input.Controller;
using UnityEngine;

namespace ControlProto.Scripts.Camera {
    public class CameraMovement : MonoBehaviour {
        [SerializeField] private Transform playerBody;
        [SerializeField] private Globals globals;

        private const float MaxVerticalLookAngle = 90;
        private const float MinVerticalLookAngle = 90;

        private readonly ControllerHandler controllerHandler = new();
        private float horizontalRotation = 0.0f;

        void Start() {
        }

        void Update() {
            float horizontalMouseMovementValue = controllerHandler.HorizontalMouseInput() * globals.HorizontalMouseSensitivity * Time.deltaTime;
            float verticalMouseMovementValue = controllerHandler.VerticalMouseInput() * globals.VerticalMouseSensitivity * Time.deltaTime;

            horizontalRotation -= verticalMouseMovementValue;

            // Don't allow character to look up more than 90 degrees, nor look down more than 90 degrees
            horizontalRotation = Mathf.Clamp(horizontalRotation, Maths.NegativeValue(MinVerticalLookAngle), Maths.PositiveValue(MaxVerticalLookAngle));

            transform.localRotation = Quaternion.Euler(horizontalRotation, 0.0f, 0.0f);

            // Here we rotate around the up direction by the mouseX amount
            playerBody.Rotate(Vector3.up * horizontalMouseMovementValue);
        }
    }
}
