using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

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
    private bool isHoldingDown = false;
    private bool isHoldingLeft = false;
    private bool isHoldingRight = false;
    private bool isUpsideDown = false;
    private float gravMultiplier = 1;

    PlayerControls controls;

    private void Awake() {
        controls = new PlayerControls();

        controls.Platformer.Jump.performed += ctx => Jump();
        controls.Platformer.Boost.performed += ctx => Boost();

        controls.Platformer.Duck.performed += ctx => {isHoldingDown = true;};
        controls.Platformer.Duck.canceled += ctx => {isHoldingDown = false;};

        controls.Platformer.MoveRight.performed += ctx => {isHoldingRight = true;};
        controls.Platformer.MoveRight.canceled += ctx => {isHoldingRight = false;};

        controls.Platformer.MoveLeft.performed += ctx => {isHoldingLeft = true;};
        controls.Platformer.MoveLeft.canceled += ctx => {isHoldingLeft = false;};

        controls.Platformer.Restart.performed += ctx => RestartLevel();
    }

    private void OnEnable() {
        controls.Platformer.Enable();
    }

    private void OnDisable() {
        controls.Platformer.Disable();
    }

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

            Movement();
            Ducking();
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

    // Handles jumping when the player presses the jump button
    private void Jump() {
        if(gc.isGrounded) {
            rb.gravityScale = midairGravScale;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    // Called in Update()
    // Sets direction of player based on player input
    // Checks if the player is holding in the direction opposite of their movement
    // If so, increase drag so player skids to a stop
    private void Movement() {
        // Direction
        if(isHoldingRight && !isFacingRight) {
            // face right
            isFacingRight = true;
            sr.flipX = true;
        } else if(isHoldingLeft && isFacingRight) {
            // face left
            isFacingRight = false;
            sr.flipX = false;
        }
        
        if((rb.velocity.x > 0 && isHoldingLeft) || (rb.velocity.x < 0 && isHoldingRight)) {
            rb.drag = dragWhenStopping;
        } else {
            rb.drag = 0;
        }
    }

    // Handles ducking when the player holds the down button
    private void Ducking() {
        if(isHoldingDown && gc.isGrounded && !isUpsideDown) {
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

    // Handles boosting when the player presses the boost button
    private void Boost() {
        if(canBoost) {
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

    // Called when player presses restart button (R)
    private void RestartLevel() {
        // Reload scene
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
    }

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

    // Flips gravity upside down/back to normal depending on isFlipped
    // Applies a gravity multiplier (mult) to make gravity less/more intense
    public void SetGravity(bool setFlipped, float mult) {
        if(setFlipped != isUpsideDown) {
            isUpsideDown = !isUpsideDown;
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * -1, transform.localScale.z);
            midairGravScale *= -1;
            slopeAccel *= -1;
            slopeAccelUp *= -1;
            duckGravScale *= -1;
            jumpForce *= -1;
        }

        // Reverse previous multiplier and apply new multiplier
        midairGravScale = (midairGravScale / gravMultiplier) * mult;
        gravMultiplier = mult;
    }
}
