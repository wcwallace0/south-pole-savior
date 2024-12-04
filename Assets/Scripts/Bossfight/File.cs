using System;
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

    public Folder parent;
    public GameObject[] fileDependencies;

    public GameObject[] dependents;

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

        foreach(GameObject dep in dependents) {
            File fl = dep.GetComponent<File>();
            Folder fld = dep.GetComponent<Folder>();
            if (fl != null) fl.UpdateIsVulnerable();
            if (fld != null) fld.UpdateIsBombable();
        }

    }


    public void UpdateIsVulnerable() {
        Debug.Log("UpdateIsVulnerable called");
        bool newValue = true;
        foreach(GameObject file in fileDependencies) {
            File fl = file.GetComponent<File>();
            Folder fld = file.GetComponent<Folder>();

            if(fl != null && !fl.isCorrupted) {
                newValue = false;
            } else if(fld != null && !fld.isBombed) {
                newValue = false;
            }
        }

        isVulnerable = newValue;
    }
}
