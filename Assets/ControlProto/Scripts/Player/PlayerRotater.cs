using System;
using System.Collections;
using System.Collections.Generic;
using ControlProto.Scripts.Global;
using ControlProto.Util;
using ControlProto.Util.Input.Controller;
using ControlProto.Util.Rotation;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ControlProto.Scripts.Player {
    public class PlayerRotater : MonoBehaviour {
        [SerializeField] private Globals globals;
        [SerializeField] private Transform cameraTransform;

        private ControllerHandler controllerHandler;
        private const float MaxPitch = 90;
        private const float MinPitch = 90;

        // Pitch is up and down rotation of an object. Like tilting your head forward and backward
        private float pitch = 0.0f;

        // Yaw refers to left and right rotation of an object. For example, turning your head and looking left or right
        private float yaw = 0.0f;

        private float horizontalMouseSensitivity;
        private float verticalMouseSensitivity;

        private void Start() {
            Mouse mouse = InputSystem.GetDevice<Mouse>();
            Keyboard keyboard = InputSystem.GetDevice<Keyboard>();

            controllerHandler = new ControllerHandler(mouse, keyboard);
            horizontalMouseSensitivity = globals.HorizontalMouseSensitivity;
            verticalMouseSensitivity = globals.VerticalMouseSensitivity;
        }

        private void LateUpdate() {
            yaw += Input.GetAxisRaw("Mouse X") * horizontalMouseSensitivity;
            pitch -= Input.GetAxisRaw("Mouse Y") * verticalMouseSensitivity;
            pitch = Mathf.Clamp(pitch, Maths.NegativeValue(MinPitch), Maths.PositiveValue(MaxPitch));

            // Rotate camera up and down
            Rotations.RotateLocally(cameraTransform, RotationAxis.XAxis, pitch);

            // Rotate player left and right
            Rotations.RotateGlobally(transform, RotationAxis.YAxis, yaw);
        }
    }
}
