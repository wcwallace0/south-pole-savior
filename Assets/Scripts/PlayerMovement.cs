using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public GroundCheck gc;
    public SpriteRenderer sr;
    public CapsuleCollider2D hitbox;
    public float xVelocity;

    [Header("Movement Parameters")]
    public float maxVelocity;
    public float boostSpeed;
    public float jumpForce;
    public float dragWhenStopping;
    public float midairGravScale;
    public float slopeGravScale;
    public float duckGravScale;

    [Header("Sprites")]
    public Sprite sliding;
    public Sprite jumping;
    public Sprite ducking;

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

        // Set sprites accordingly
        if(gc.isGrounded) {
            sr.sprite = sliding;
        } else {
            sr.sprite = jumping;
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
            sr.flipX = true;
        } else if(Input.GetKeyDown(KeyCode.A) && isFacingRight) {
            // face left
            isFacingRight = false;
            sr.flipX = false;
        }

        // Slow player down when they hold in direction opposite of movement
        if((rb.velocity.x > 0 && Input.GetKey(KeyCode.A)) || (rb.velocity.x < 0 && Input.GetKey(KeyCode.D))) {
            rb.drag = dragWhenStopping;
        } else {
            rb.drag = 0;
        }

        // Ducking
        if(Input.GetKey(KeyCode.S) && gc.isGrounded) {
            rb.gravityScale = duckGravScale;
            sr.sprite = ducking;

            // Make hitbox smaller
            hitbox.offset = new Vector2(hitbox.offset.x, -0.2f);
            hitbox.size = new Vector2(hitbox.size.x, 0.6f);
        } else {
            // Set gravity scale for when not ducking
            if(gc.isGrounded) {
                rb.gravityScale = slopeGravScale;
            } else {
                rb.gravityScale = midairGravScale;
            }

            // Set hitbox back to normal size
            hitbox.offset = new Vector2(0f, 0f);
            hitbox.size = new Vector2(0.5f, 1f);

        }

        // Boost
        if(Input.GetKeyDown(KeyCode.Mouse1)) {
            Vector2 boost = new Vector2(boostSpeed, rb.velocity.y);

            if(isFacingRight) {
                // Boost should not slow down player
                if(rb.velocity.x > boost.x) {
                    boost.x = rb.velocity.x;
                }
            } else {
                boost.x *= -1;
                // Boost should not slow down player
                if(rb.velocity.x < boost.x) {
                    boost.x = rb.velocity.x;
                }
            }
            rb.velocity = boost;
        }
    }
}
