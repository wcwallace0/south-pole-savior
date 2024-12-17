using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
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

    public LabelManager lm;

    void Start(){
        // lm = FindObjectOfType<LabelManager>();
        // if (gameObject.GetComponent<Image>().enabled){
        //     lm.AddObject(gameObject);
        // }
        UpdateIsVulnerable();
        GetComponent<Image>().sprite = normal;
    }

    public void SetSelected(bool isSelected) {
        // change sprite to selected
        if(!isCorrupted) {
            GetComponent<Image>().sprite = isSelected ? selected : normal;
        }
    }

    public void SetCorrupted(bool corrupt) {
        
        GetComponent<Button>().enabled = !corrupt;
        //Debug.Log("boolean status in SetCorrupted: " + corrupt);
        isCorrupted = corrupt;
        //Debug.Log("boolean status in SetCorrupted: " + corrupt);


        //TODO: This line (line 45) is not working and I (Colin) haven't been able to figure out why (hence the debug logs and test string).
        //I've tried a few different things but there may be something obvious I'm just not seeing,
        //I'm going to sleep on it and see if I can figure it out later, but if anyone else sees this
        //feel free to give it a show.
        GetComponent<Image>().sprite = corrupt ? corrupted : normal;



        String test = corrupt ? "corrupted" : "normal";
        //Debug.Log("File sprite: " + test);

        if(corrupt) {
            Cybersecurity.corruptedFiles.Add(this);
        } else {
            Cybersecurity.corruptedFiles.Remove(this);
        }

        foreach(GameObject dep in dependents) {
            File fl = dep.GetComponent<File>();
            Folder fld = dep.GetComponent<Folder>();
            if (fl != null) fl.UpdateIsVulnerable();
            if (fld != null) fld.UpdateIsBombable();
        }

    }


    public void UpdateIsVulnerable() {
        //Debug.Log("UpdateIsVulnerable called");
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
