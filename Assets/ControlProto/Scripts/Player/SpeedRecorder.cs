using UnityEngine;

namespace ControlProto.Scripts.Player {
    public class SpeedRecorder : MonoBehaviour {
        private Vector3 previousPosition;
        private float previousTime;
        private float maxRecordedSpeed = 0f;

        void Start() {
            previousPosition = transform.position;
            previousTime = Time.time;
        }

        void Update() {
            Vector3 currentPosition = transform.position;
            Vector3 distanceTravelled = currentPosition - previousPosition;
            float timeElapsed = Time.time - previousTime;
            float recordedSpeed = distanceTravelled.magnitude / timeElapsed;

            if (recordedSpeed > maxRecordedSpeed) {
                maxRecordedSpeed = recordedSpeed;
            }

            previousPosition = currentPosition;
            previousTime = Time.time;

            Debug.Log("Speed: " + recordedSpeed);
            Debug.Log("maxSpeed: " + maxRecordedSpeed);
        }
    }
}
