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
    public bool isInBombed = false;

    public Sprite normal;
    public Sprite selected;
    public Sprite corrupted;

    public Folder parent;
    public GameObject[] fileDependencies;

    public GameObject[] dependents;

    public LabelManager lm;
    public Cybersecurity cybersec;
    public Alert alert;
    public PlayerActions player;
    void Start(){
        lm = FindObjectOfType<LabelManager>();
        alert = FindObjectOfType<Alert>();
        cybersec = FindObjectOfType<Cybersecurity>();
        // if (gameObject.GetComponent<Image>().enabled){
        //     lm.AddObject(gameObject);
        // }
        UpdateIsVulnerable();
        GetComponent<Image>().sprite = normal;
    }

    public void SetSelected(bool isSelected) {
        // change sprite to selected
        if(!isCorrupted) {
            if(isSelected){
                GetComponent<Image>().sprite = selected;
            } else{
                GetComponent<Image>().sprite = normal;
            }
            //GetComponent<Image>().sprite = isSelected ? selected : normal;
        }
    }

    public void SetCorrupted(bool corrupt) {
        
        GetComponent<Button>().enabled = !corrupt;
        isCorrupted = corrupt;
        
        GetComponent<Image>().sprite = corrupt ? corrupted : normal;

        if(corrupt) {
            Cybersecurity.corruptedFiles.Add(this);
            if(this.name == "Tracker")
            {
                cybersec.ipTimer += 2f;
                alert.DisplayAlert(alert.ipNerf);
            }
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
        //Debug.Log("UpdateIsVulnerable c`led");
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
