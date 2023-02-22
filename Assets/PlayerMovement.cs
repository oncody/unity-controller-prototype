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
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Get the collider component
        Collider collider = GetComponent<Collider>();

        float topPosition = transform.position.y + collider.bounds.extents.y;
        float bottomPosition = transform.position.y - collider.bounds.extents.y;
        float characterHeight = topPosition - bottomPosition;

        Debug.Log($"topPosition: {topPosition}");
        Debug.Log($"bottomPosition: {bottomPosition}");
        Debug.Log($"characterHeight: {characterHeight}");
        
        RaycastHit hit;
        
        float sphereRadius = 0.2f;
        Vector3 spherePosition = new Vector3(transform.position.x, bottomPosition, transform.position.z);
        
        Debug.Log($"spherePosition: {spherePosition}");
        
        float sphereDistance = controller.radius + sphereRadius;
        Debug.Log($"sphereDistance: {sphereDistance}");
        Debug.Log($"sphereRadius: {sphereRadius}");
        
        
        // Set up the ray's parameters
        // Vector3 rayOrigin = transform.position + Vector3.up * 0.1f; // Set the origin of the ray to the center of the character's position
        float rayDistance = characterHeight / 2.0f + 0.2f; // Set the distance of the ray to be slightly larger than the character's height
        
        
        // isGrounded = Physics.SphereCast(spherePosition, sphereRadius, -Vector3.up, out RaycastHit hitInfo, sphereDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore);
        // isGrounded = Physics.Raycast(spherePosition, Vector3.down, out hit, distance, layerMask);
        // isGrounded = Physics.Raycast(spherePosition, Vector3.down, out hit, distance, Physics.AllLayers);
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
