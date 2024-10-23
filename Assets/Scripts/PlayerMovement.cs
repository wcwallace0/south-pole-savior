using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float maxVelocity;
    public float jumpForce;
    public GroundCheck gc;

    private bool onSlope;

    private void Start() {
        rb.velocity = new Vector3(7f, 0f, 0f);
    }

    private void Update() {
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVelocity);

        if(gc.isGrounded && Input.GetKeyDown(KeyCode.Space)) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
}
