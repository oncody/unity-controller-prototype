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

        // Get the top position of the object
        float topPosition = transform.position.y + collider.bounds.extents.y;

        // Get the bottom position of the object
        float bottomPosition = transform.position.y - collider.bounds.extents.y;


        Debug.Log($"topPosition: {topPosition}");
        Debug.Log($"bottomPosition: {bottomPosition}");
        
        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, distance, layerMask);
        
        Debug.Log($"isGrounded: {isGrounded}");

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
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
