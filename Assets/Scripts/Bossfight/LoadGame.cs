using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{
    public List<GameObject> startingObs;

    public List<GameObject> introObs;
    public PlayerActions player;
    public Cybersecurity cybersec;
    public Alert alert;

    
    // Start is called before the first frame update
    void Start()
    {
        StopGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        Debug.Log("StartGame called");
        foreach(GameObject obj in introObs)
        {
            obj.SetActive(false);
        }

        foreach(GameObject obj in startingObs)
        {
            obj.SetActive(true);
        }
    }

    public void StopGame()
    {
        player.KillCoroutines();
        cybersec.KillCoroutines();
        alert.KillCoroutines();
        foreach(GameObject obj in startingObs)
        {
            obj.SetActive(false);
        }
    }

    public void EndGame(bool win)
    {
        StopGame();
        if (win){
            SceneManager.LoadScene("Cyberfight Victory");
        } else{
            SceneManager.LoadScene("Cyberfight Defeat");
        }
    }
}
