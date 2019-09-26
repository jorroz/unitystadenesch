using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float MoveSpeed = 5f;

    // Reference to Rigidbody
    public Rigidbody2D rb;

    // Movement Vector2; used for detecting player input (x, y)
    private Vector2 movement;

    // Function that gets called upon start
    private void Start()
    {
        // Grabbing the Rigidbody2D component from the current gameObject
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Function that gets called every frame
    void Update()
    {
        // Gets a value of 1 (right) or -1 (left) based on player input; See: InputManager
        movement.x = Input.GetAxisRaw("Horizontal");  // Input on Horizontal Axis
        movement.y = Input.GetAxisRaw("Vertical");  // Input on Vertical Axis
    }

    // Same as Update() but for application of the PhysicsEngine
    void FixedUpdate()
    {
        // Adding movement to the current position and multiplying it by the movespeed
        rb.MovePosition(rb.position + movement * MoveSpeed * Time.fixedDeltaTime);
    }
}
