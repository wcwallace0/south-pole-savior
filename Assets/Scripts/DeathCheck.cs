using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCheck : MonoBehaviour
{
    public PlayerMovement pm;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Slope")) {
            pm.Death();
        }
    }
}
