using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Unity.VisualScripting;
using UnityEditor.AssetImporters;
using UnityEngine;
using UnityEngine.UI;

public class Folder : MonoBehaviour
{
    public LoadGame loader;
    public string folderName;
    public int rows = 2;
    public int cols = 3;
    public float spacing;
    public float padding;
    public RectTransform rectTransform;
    public bool isRoot;

    public Sprite normalSprite;
    public Sprite corruptedSprite;

    [HideInInspector]
    public GameObject[,] grid;
    public bool isBombable;
    public bool isBombed;
    public GameObject[] fileDependencies;
    public GameObject[] dependents;
    public LabelManager lm;
    public Cybersecurity cybersec;


    private void Start() {
        UpdateIsBombable();
        lm = FindObjectOfType<LabelManager>();
        cybersec = FindObjectOfType<Cybersecurity>();
        loader = FindObjectOfType<LoadGame>();
        // if (gameObject.GetComponent<Image>().enabled){
        //     lm.AddObject(gameObject);
        // }
        GetComponent<Image>().sprite = normalSprite;
        grid = new GameObject[rows,cols];

        // Fill the grid with the children of this gameobject
        int i = 0;
        int j = 0;
        foreach(RectTransform child in transform) {
            grid[i,j] = child.gameObject;
            
            if(j >= cols-1) {
                i++;
                j = 0;
            } else {
                j++;
            }

            if(i >= rows) {
                Debug.LogWarning("Could not add all child GameObjects to grid. Please increase row or col count on " + gameObject.name);
                break;
            }
        }

        PositionFiles();
    }

    // Navigates the UI to this folder 
    // (shows folders and files in the grid object on this script)
    // Takes a Folder argument specifying what folder the player is navigating from
    public void Navigate(Folder previous) {
        rectTransform.anchoredPosition = new Vector2(0f, 0f);
        previous.SetAllVisible(false);

        PositionFiles();
        SetAllVisible(true);
    }

    private void PositionFiles() {
        List<Text> labels = lm.labels;
        float offX = lm.offset.x;
        float offY = lm.offset.y;
        for(int i = 0; i<rows; i++) {
            for(int j = 0; j<cols; j++) {
                GameObject file = grid[i,j];
                if(file != null) {
                    RectTransform fileRect = file.GetComponent<RectTransform>();
                    float step = spacing + fileRect.rect.width;
                    fileRect.anchoredPosition = new Vector2((j*step) + padding, (-i*step) - padding);
                    
                    foreach(Text lbl in labels)
                    {
                        if (lbl.name == file.name + "_Label"){
                            RectTransform lblRect = lbl.GetComponent<RectTransform>();
                            
                            lblRect.anchoredPosition = new Vector2((j*step) + padding + offX, (-i*step) - padding + offY);
                        }
                    }
                }
            }
        }
    }

    // Enables/disables all GameObjects in the grid
    public void SetAllVisible(bool isActive) {
        foreach(GameObject file in grid) {
            if(file != null) {
                file.GetComponent<Image>().enabled = isActive;
                
                Button b = file.GetComponent<Button>();
                Folder f = file.GetComponent<Folder>();
                if(f != null && !f.isBombed) {
                    b.enabled = isActive;
                } else if(f == null) {
                    b.enabled = isActive;
                }
            }
        }
    }

    public void Corrupt() {
        if(isBombable) {
            if (this.name == "System" || this.name == "Root") { loader.EndGame(true); }
            if(this.name == "Security") { 
                cybersec.maxPoints ++; 
                if(!cybersec.isPwned){
                    cybersec.actionPoints = cybersec.maxPoints;
                }
                cybersec.endgame = true;
            }
            if(this.name == "Permissions Folder"){ cybersec.endgame = true;}
            gameObject.GetComponent<Image>().sprite = corruptedSprite;
            gameObject.GetComponent<Button>().enabled = false;
            this.name = "CORRUPTED";
            isBombed = true;

            List<GameObject> children = new List<GameObject>();

            foreach(Transform child in transform)
            {
                File fl = child.GetComponent<File>();
                Folder fld = child.GetComponent<Folder>();
                if (fl != null) { 
                    fl.SetCorrupted(true);
                    fl.isInBombed = true;
                }
                if (fld != null) { fld.Corrupt(); }
            }

            foreach(GameObject dep in dependents) {
            File fl = dep.GetComponent<File>();
            Folder fld = dep.GetComponent<Folder>();
            if (fl != null) fl.UpdateIsVulnerable();
            if (fld != null) fld.UpdateIsBombable();
        }
        }
    }

    public void UpdateIsBombable() {
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

        isBombable = newValue;
    }
}
