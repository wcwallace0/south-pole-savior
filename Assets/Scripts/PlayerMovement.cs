using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform playerTransform;
    public float jumpForce = 400f;
    // public GameObject diagonal;
    
    private bool onSpline = false;

    public SplineContainer spline;
    private float splineLength;
    private float speed;
    public float maxVelocity = 5f;
    public float accelRate = 0.5f;
    private float distancePercentage = 0f;

    private void Start() {
        splineLength = spline.CalculateLength();

        // Enter spline
        onSpline = true;
        rb.gravityScale = 0;
    }

    private void FixedUpdate() {
        if(onSpline) {
            distancePercentage += speed * Time.deltaTime / splineLength;
            playerTransform.position = spline.EvaluatePosition(distancePercentage);
            Accelerate();
        }
    }

    private void Accelerate() {
        // TODO determine angle of slope and apply force (acceleration) accordingly
        // apply less acceleration the closer you are to maxVelocity? if so, wouldn't need if statement below
        speed += accelRate;

        if(speed > maxVelocity) {
            speed = maxVelocity;
        } else if(speed < -maxVelocity) {
            speed = -maxVelocity;
        }
    }

    private void EnterSpline() {
        onSpline = true;

        // TODO check each position along the spline at some interval (hopefully not too small)
        // get the position closest to the player position and snap the player to that
        // you can stop iterating once the distance starts decreasing?
    }

    private void ExitSpline() {
        onSpline = false;
    }

    // private void FixedUpdate() {
    //     if(onSlope) {
    //         float val = (maxVelocity-Math.Abs(rb.velocity.x)) * accelRate;
    //         rb.AddForce(new Vector2(-val, -val));
    //     }
    // }

    // private void Update() {
    //     if(Input.GetKeyDown(KeyCode.Space)) {
    //         rb.AddForce(new Vector2(0f, jumpForce));
    //     }
    // }

    // private void OnTriggerEnter2D(Collider2D other) {
    //     if(other.gameObject.CompareTag("Slope")) {
    //         gameObject.transform.SetParent(diagonal.transform);
    //         // rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
    //         rb.gravityScale = 0;
    //         onSlope = true;
    //     }
    // }

    // private void OnTriggerExit2D(Collider2D other) {
    //     if(other.gameObject.CompareTag("Slope")) {
    //         gameObject.transform.SetParent(null);
    //         // rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    //         rb.gravityScale = 2;
    //         onSlope = false;
    //     }
    // }
}
