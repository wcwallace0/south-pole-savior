using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityField : MonoBehaviour
{
    public PlayerMovement pm;
    public bool isFlipped;
    public float gravMultiplier;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")) {
            pm.SetGravity(isFlipped, gravMultiplier);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")) {
            pm.SetGravity(false, 1);
        }
    }
}
