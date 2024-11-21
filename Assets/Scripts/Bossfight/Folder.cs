using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Folder : MonoBehaviour
{
    public string folderName;
    public int rows = 2;
    public int cols = 3;
    public float spacing;
    public RectTransform rectTransform;
    public bool isRoot;

    public Sprite normalSprite;
    public Sprite corruptedSprite;

    [HideInInspector]
    public GameObject[,] grid;
    public bool isBombable;
    public bool isBombed;

    private void Start() {
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
        for(int i = 0; i<rows; i++) {
            for(int j = 0; j<cols; j++) {
                GameObject file = grid[i,j];
                if(file != null) {
                    RectTransform fileRect = file.GetComponent<RectTransform>();
                    float step = spacing + fileRect.rect.width;
                    fileRect.anchoredPosition = new Vector2(j*step, -i*step);
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
        gameObject.GetComponent<Image>().sprite = corruptedSprite;
        gameObject.GetComponent<Button>().enabled = false;
        isBombed = true;
    }
}
