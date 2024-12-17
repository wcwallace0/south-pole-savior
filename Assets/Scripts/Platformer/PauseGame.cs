using UnityEngine;

public class PauseGame : MonoBehaviour
{
    private bool isPaused = false; // Track if the game is paused

    void Update()
    {
        // Check for the Escape key to toggle pause
        if (Input.GetKeyDown(KeyCode.P)) 
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                Pause();
            }
        }
    }

    // Function to pause the game
    public void Pause()
    {
        Time.timeScale = 0f; // Freeze the game
        isPaused = true;
        Debug.Log("Game Paused");
    }

    // Function to resume the game
    public void ResumeGame()
    {
        Time.timeScale = 1f; // Resume the game
        isPaused = false;
        Debug.Log("Game Resumed");
    }
}
