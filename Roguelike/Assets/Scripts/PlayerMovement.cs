using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float MoveSpeed = 5f;
    public float playerHealth = 100f;

    // Reference to Rigidbody
    public Rigidbody2D rb;
    public Camera cam;

    // Movement Vector2; used for detecting player input (x, y)
    private Vector2 movement;
    Vector2 mousePos;

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

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    // Same as Update() but for application of the PhysicsEngine
    void FixedUpdate()
    {
        // Adding movement to the current position and multiplying it by the movespeed
        rb.MovePosition(rb.position + movement * MoveSpeed * Time.fixedDeltaTime);

        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }
}
