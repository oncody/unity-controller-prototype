using System.Collections;
using System.Collections.Generic;
using ControlProto.Scripts.Global;
using ControlProto.Util;
using ControlProto.Util.Input.Controller;
using ControlProto.Util.Rotation;
using UnityEngine;

namespace ControlProto.Scripts.Player {
    public class PlayerRotater : MonoBehaviour {
        [SerializeField] private Globals globals;
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Transform cameraTransform;

        private readonly ControllerHandler controllerHandler = new();
        private const float MaxPitch = 90;
        private const float MinPitch = 90;

        // Pitch is up and down rotation of an object. Like tilting your head forward and backward
        private float pitch = 0.0f;

        // Yaw refers to left and right rotation of an object. For example, turning your head and looking left or right
        private float yaw = 0.0f;

        private void LateUpdate() {
            yaw += controllerHandler.HorizontalMouseMovement() * globals.HorizontalMouseSensitivity;
            pitch -= controllerHandler.VerticalMouseMovement() * globals.VerticalMouseSensitivity;
            pitch = Mathf.Clamp(pitch, Maths.NegativeValue(MinPitch), Maths.PositiveValue(MaxPitch));

            // Rotate camera up and down
            Rotations.RotateLocally(cameraTransform, RotationAxis.XAxis, pitch);

            // Rotate player left and right
            Rotations.RotateGlobally(transform, RotationAxis.YAxis, yaw);
        }
    }
}
