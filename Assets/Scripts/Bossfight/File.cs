using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class File : MonoBehaviour
{
    File fl;
    public string fileName;
    public bool isVulnerable = false;
    public bool isDeleted = false;
    public Image image;

    public void RestoreFile(){
        isDeleted = false;
        //Restore the file sprite
        Debug.Log(fileName + " has been restored by cybersecurity.");
    }
}
