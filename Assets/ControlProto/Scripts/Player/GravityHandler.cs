using System;
using ControlProto.Scripts.Global;
using ControlProto.Util;
using UnityEngine;

namespace ControlProto.Scripts.Player {
    public class GravityHandler : MonoBehaviour {
        [SerializeField] private Globals globals;
        [SerializeField] private CharacterController characterController;

        private bool isGrounded;
        private Vector3 velocity;

        private void Start() {
            // GatherInputs();
        }

        private void Update() {
            // GatherInputs();
        }

        private void GatherInputs() {
            // Set the distance of the ray to be slightly larger than the character's height
            // float rayDistance = (characterController.height / 2.0f) + Maths.PositiveValue(globals.GroundCheckDistance);
            // isGrounded = Physics.SphereCast(transform.position, characterController.radius, Vector3.down, out _, rayDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore);
        }

        private void FixedUpdate() {
            // if (isGrounded && (velocity.y < 0)) {
            //     velocity.y = -2f;
            // }
            //
            // velocity.y -= Maths.PositiveValue(globals.Gravity) * Time.deltaTime;
            // characterController.Move(velocity * Time.deltaTime);
        }
    }
}