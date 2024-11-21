using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class File : MonoBehaviour
{
    File fl;
    public string fileName;
    public bool isVulnerable = false;
    public bool isCorrupted = false;

    public Sprite normal;
    public Sprite selected;
    public Sprite corrupted;

    public void SetSelected(bool isSelected) {
        // change sprite to selected
        GetComponent<Image>().sprite = isSelected ? selected : normal;
    }

    public void SetCorrupted(bool corrupt) {
        GetComponent<Button>().enabled = !corrupt;
        isCorrupted = corrupt;
        GetComponent<Image>().sprite = corrupt ? corrupted : normal;

        if(corrupt) {
            Cybersecurity.corruptedFiles.Add(this);
            Debug.Log(fileName + " has been corrupted by player.");
        } else {
            Cybersecurity.corruptedFiles.Remove(this);
            Debug.Log(fileName + " has been restored by cybersecurity.");
        }
    }
}
