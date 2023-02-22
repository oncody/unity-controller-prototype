using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float distance = 0.4f;
    public LayerMask layerMask;
    public CharacterController controller;

    public float speed = 12f;

    public float gravity = -9.81f;
    private Vector3 velocity;

    private bool isGrounded;
    
    private Vector3 previousPosition;
    private float previousTime;

    private float maxSpeed = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        previousPosition = transform.position;
        previousTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the distance travelled since the last frame
        Vector3 distanceTravelled = transform.position - previousPosition;

        // Calculate the time elapsed since the last frame
        float deltaTime = Time.time - previousTime;

        // Calculate the speed as the distance divided by the time
        float thisSpeed = distanceTravelled.magnitude / deltaTime;

        if (thisSpeed > maxSpeed)
        {
            maxSpeed = thisSpeed;
        }

        // Store the current position and time for the next frame
        previousPosition = transform.position;
        previousTime = Time.time;

        Debug.Log("Speed: " + thisSpeed);
        Debug.Log("maxSpeed: " + maxSpeed);
        
        
        // Get the collider component
        Collider collider = GetComponent<Collider>();

        float topPosition = transform.position.y + collider.bounds.extents.y;
        float bottomPosition = transform.position.y - collider.bounds.extents.y;
        float characterHeight = topPosition - bottomPosition;

        // Debug.Log($"topPosition: {topPosition}");
        // Debug.Log($"bottomPosition: {bottomPosition}");
        Debug.Log($"characterHeight: {characterHeight}");
        
        RaycastHit hit;
        float rayDistance = characterHeight / 2.0f + 0.2f; // Set the distance of the ray to be slightly larger than the character's height
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, rayDistance, Physics.AllLayers);

        Debug.Log($"isGrounded: {isGrounded}");

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0;
        }
        
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 directionToMoveTo = transform.right * x + transform.forward * z;
        controller.Move(directionToMoveTo * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void FixedUpdate()
    {
    }
    
    public bool IsGrounded()
    {
        return isGrounded;
    }
}
