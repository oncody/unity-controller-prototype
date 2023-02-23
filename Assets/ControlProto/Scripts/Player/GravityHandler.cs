using System.Collections;
using System.Collections.Generic;
using ControlProto.Scripts.Global;
using ControlProto.Util;
using UnityEngine;

namespace ControlProto.Scripts.Player {
    public class GravityHandler : MonoBehaviour {
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Globals globals;
        private bool isGrounded;
        private Vector3 velocity;

        void Start() {
        }

        void Update() {
            Transform currentTransform = transform;
            // Set the distance of the ray to be slightly larger than the character's height
            float rayDistance = (characterController.height / 2.0f) + Maths.PositiveValue(globals.GroundCheckDistance);
            isGrounded = Physics.SphereCast(currentTransform.position, characterController.radius, Vector3.down, out _, rayDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            // Debug.Log($"isGrounded: {isGrounded}");

            if (isGrounded && velocity.y < 0) {
                velocity.y = -2;
            }

            velocity.y -= Maths.PositiveValue(globals.Gravity) * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);
        }
    }
}
