using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadGame : MonoBehaviour
{
    public List<GameObject> startingObs;

    public List<GameObject> introObs;
    public PlayerActions player;
    public Cybersecurity cybersec;
    public Alert alert;

    private int clickCt;

    
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
        foreach(GameObject obj in startingObs)
        {
            obj.SetActive(false);
        }
    }

    public void EndGame(bool win)
    {
        //StopGame();
        if (win){
            SceneManager.LoadScene("Cyberfight Victory");
        } else{
            SceneManager.LoadScene("Cyberfight Defeat");
        }
    }

    public void RestartButton()
    {
        SceneManager.LoadScene("Cyberfight");
    }
    public void QuitButton()
    {
        clickCt ++;
        if (clickCt == 2)
        {
            SceneManager.LoadScene("Menu");
        } else if (clickCt == 1)
        {
            StartCoroutine(QuitTimer());
        }
    }

    IEnumerator QuitTimer()
    {
        Button quit = GameObject.Find("Button_Quit").GetComponent<Button>();
        TextMeshProUGUI quitText = GameObject.Find("Text_Quit").GetComponent<TextMeshProUGUI>();
        Image buttImage = quit.GetComponent<Image>();


        string currText = quitText.text;
        Color currColor = buttImage.color;

        quitText.text = "Are you sure?";
        quit.GetComponent<Image>().color = Color.red;

        yield return new WaitForSeconds(5f);

        quitText.text = currText;
        buttImage.color = currColor;
        

    }
}
