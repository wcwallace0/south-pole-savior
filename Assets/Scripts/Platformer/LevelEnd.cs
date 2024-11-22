using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    public string sceneToLoad; // name of scene to be loaded upon completion of level

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")) {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
