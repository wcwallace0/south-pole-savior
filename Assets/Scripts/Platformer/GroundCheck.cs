using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool isGrounded = false;
    public Animator plAnim;
    public bool jumped;
    public PlayerMovement pm;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Slope")) {
            isGrounded = true;
            plAnim.SetBool("Falling", false);
            jumped = false;
            pm.UpdateGravityScale();
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.CompareTag("Slope")) {
            isGrounded = false;
            plAnim.SetBool("Falling", true);
            pm.UpdateGravityScale();
        }
    }
}
