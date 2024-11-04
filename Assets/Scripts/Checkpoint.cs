using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public PlayerMovement pm;
    public Vector2 spawnPosition;
    public bool direction; // direction that the player faces when respawning at this checkpoint

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")) {
            pm.Checkpoint(spawnPosition, direction);
        }
    }
}
