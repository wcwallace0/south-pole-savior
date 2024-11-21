using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject menuObject;

    public void StartGame() {
        SceneManager.LoadScene("TutorialLevel");
    }

    public void OpenPanel(GameObject panel) {
        menuObject.SetActive(false);
        panel.SetActive(true);
    }

    public void ClosePanel(GameObject panel) {
        menuObject.SetActive(true);
        panel.SetActive(false);
    }
}
