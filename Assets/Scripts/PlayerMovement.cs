using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float jumpForce = 400f;
    // public GameObject diagonal;

    public float maxVelocity = 5f;
    public float accelRate = 0.5f;
    
    private bool onSlope = false;

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
