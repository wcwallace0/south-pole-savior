using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public GroundCheck gc;
    public DeathCheck dc;
    public SpriteRenderer sr;
    public CapsuleCollider2D hitbox;
    public CapsuleCollider2D hitboxSmall;
    public BoxCollider2D deathbox;
    public BoxCollider2D deathboxSmall;
    public float xVelocity;
    public GameObject boostIndicator;

    [Header("Movement Parameters")]
    public float maxVelocity;
    public float boostSpeed;
    public float boostCooldown;
    public float jumpForce;
    public float dragWhenStopping;
    public float midairGravScale; // Gravity scale for jumping and falling
    public float slopeAccel; // Gravity scale on slopes when player is sliding down
    public float slopeAccelUp; // Gravity scale on slopes when player is moving up the slope
    public float duckGravScale; // Gravity scale on slopes when player is holding down

    [Header("Other Parameters")]
    public float deathTime;
    private Vector2 respawnPosition;
    public bool faceRightOnSpawn = true; // true - right, false - left

    [Header("Sprites")]
    public Sprite sliding;
    public Sprite jumping;
    public Sprite ducking;

    private bool isFacingRight;
    private bool isDead = false;
    private bool canBoost = true;
    private float gravMultiplier = 1;

    private void Start() {
        respawnPosition = transform.position;
        isFacingRight = faceRightOnSpawn;
        sr.flipX = faceRightOnSpawn;
    }

    private void Update() {
        if(!isDead) {
            EnforceMaxVelocity();

            // Set sprites accordingly
            if(gc.isGrounded) {
                sr.sprite = sliding;
            } else {
                sr.sprite = jumping;
            }

            // for display in inspector
            xVelocity = rb.velocity.x;

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

            Jump();
            Skidding();
            Ducking();
            Boosting();
        }
    }


    // MOVEMENT

    // Called in Update()
    // Clamps magnitude of X Velocity to maxVelocity
    private void EnforceMaxVelocity() {
        if(rb.velocity.x > maxVelocity) {
            rb.velocity = new Vector2(maxVelocity, rb.velocity.y);
        } else if(rb.velocity.x < -maxVelocity) {
            rb.velocity = new Vector2(-maxVelocity, rb.velocity.y);
        }
    }


    // Called in Update()
    // Handles jumping when the player presses the jump button
    private void Jump() {
        if(gc.isGrounded && Input.GetKeyDown(KeyCode.Space)) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    // Called in Update()
    // Checks if the player is holding in the direction opposite of their movement
    // If so, increase drag so player skids to a stop
    private void Skidding() {
        if((rb.velocity.x > 0 && Input.GetKey(KeyCode.A)) || (rb.velocity.x < 0 && Input.GetKey(KeyCode.D))) {
            rb.drag = dragWhenStopping;
        } else {
            rb.drag = 0;
        }
    }

    // Called in Update()
    // Handles ducking when the player holds the down button
    private void Ducking() {
        if(Input.GetKey(KeyCode.S) && gc.isGrounded) {
            rb.gravityScale = duckGravScale;
            sr.sprite = ducking;

            ShrinkHitboxes(true);
        } else {
            // Set gravity scale for when not ducking
            if(gc.isGrounded) {
                if(rb.velocity.y > 0) {
                    rb.gravityScale = slopeAccelUp;
                } else {
                    rb.gravityScale = slopeAccel;
                }
            } else {
                rb.gravityScale = midairGravScale;
            }

            ShrinkHitboxes(false);
        }
    }

    // Called in Update()
    // Handles boosting when the player presses the boost button
    private void Boosting() {
        if(canBoost && Input.GetKeyDown(KeyCode.Return)) {
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

            canBoost = false;
            boostIndicator.SetActive(true);
            StartCoroutine(BoostCooldown());
        }
    }

    private IEnumerator BoostCooldown() {
        yield return new WaitForSeconds(boostCooldown);
        canBoost = true;
        boostIndicator.SetActive(false);
    }

    // If shrink is true, shrinks player hitboxes (for when player is ducking)
    // If shrink is false, returns player hitboxes to their default
    private void ShrinkHitboxes(bool shrink) {
        hitbox.enabled = !shrink;
        hitboxSmall.enabled = shrink;

        deathbox.enabled = !shrink;
        deathboxSmall.enabled = shrink;
    }


    // DEATH AND CHECKPOINTS

    public void Death() {
        isDead = true;
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
        dc.enabled = false;
        sr.enabled = false;
        isFacingRight = faceRightOnSpawn;
        sr.flipX = faceRightOnSpawn;
 
        StartCoroutine(DeathProcess());
    }

    private IEnumerator DeathProcess() {
        yield return new WaitForSeconds(deathTime);

        transform.position = respawnPosition;
        rb.gravityScale = midairGravScale;
        dc.enabled = true;
        sr.enabled = true;

        // Restore player controls
        isDead = false;
    }

    public void Checkpoint(Vector2 newSpawnPosition, bool newInitialDirection) {
        respawnPosition = newSpawnPosition;
        faceRightOnSpawn = newInitialDirection;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(!isDead && other.gameObject.CompareTag("Death")) {
            Death();
        }
    }


    // GRAVITY FIELDS

    // Takes a function that is applied to all gravity related fields
    // Pass in MakeNegative() to flip gravity
    // Pass in Math.Abs() to return gravity to normal
    private void ApplyToGravFields(Func<float, float> f) {
        transform.localScale = new Vector3(transform.localScale.x, f(transform.localScale.y), transform.localScale.z);
        midairGravScale = f(midairGravScale);
        slopeAccel = f(slopeAccel);
        slopeAccelUp = f(slopeAccelUp);
        duckGravScale = f(duckGravScale);
        jumpForce = f(jumpForce);
    }

    // Makes a number negative whether it is already negative or not
    private float MakeNegative(float num) {
        return Math.Abs(num) * -1;
    }

    // Flips gravity upside down/back to normal depending on isFlipped
    // Applies a gravity multiplier (mult) to make gravity less/more intense
    public void SetGravity(bool isFlipped, float mult) {
        if(isFlipped) {
            ApplyToGravFields(MakeNegative);
        } else {
            ApplyToGravFields(Math.Abs);
        }

        // Reverse previous multiplier and apply new multiplier
        midairGravScale = (midairGravScale / gravMultiplier) * mult;
        gravMultiplier = mult;
    }
}
