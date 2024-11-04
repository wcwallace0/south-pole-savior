using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool isGrounded = false;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Slope")) {
            isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.CompareTag("Slope")) {
            isGrounded = false;
        }
    }
}
