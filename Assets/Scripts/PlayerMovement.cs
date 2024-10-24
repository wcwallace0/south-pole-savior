using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float maxVelocity;
    public float boostSpeed;
    public float jumpForce;
    public float dragWhenStopping;
    public GroundCheck gc;

    public float xVelocity;

    private bool isFacingRight = true;

    private void Start() {
        // rb.velocity = new Vector3(-7f, 0f, 0f);
    }

    private void Update() {
        // Enforce max x velocity
        if(rb.velocity.x > maxVelocity) {
            rb.velocity = new Vector2(maxVelocity, rb.velocity.y);
        } else if(rb.velocity.x < -maxVelocity) {
            rb.velocity = new Vector2(-maxVelocity, rb.velocity.y);
        }

        // for display in inspector
        xVelocity = rb.velocity.x;

        // Jump
        if(gc.isGrounded && Input.GetKeyDown(KeyCode.Mouse0)) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // Direction
        if(Input.GetKeyDown(KeyCode.D) && !isFacingRight) {
            // face right
            isFacingRight = true;
        } else if(Input.GetKeyDown(KeyCode.A) && isFacingRight) {
            // face left
            isFacingRight = false;
        }

        // Slow player down when they hold in direction opposite of movement
        if((rb.velocity.x > 0 && Input.GetKey(KeyCode.A)) || (rb.velocity.x < 0 && Input.GetKey(KeyCode.D))) {
            rb.drag = dragWhenStopping;
        } else {
            rb.drag = 0;
        }

        // Ducking
        if(Input.GetKey(KeyCode.S)) {
            // TODO duck
        }

        // Boost
        if(Input.GetKeyDown(KeyCode.Mouse1)) {
            Vector2 boost = rb.velocity;
            if(isFacingRight) {
                boost.x = boostSpeed;
            } else {
                boost.x = -boostSpeed;
            }
            rb.velocity = boost;
        }
    }
}
