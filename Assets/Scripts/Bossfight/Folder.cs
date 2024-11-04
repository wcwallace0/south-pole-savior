using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Folder : MonoBehaviour
{
    public string folderName;
    public int rows;
    public int cols;

    [HideInInspector]
    public GameObject[,] grid;
    public bool isBombable;

    private void Start() {
        grid = new GameObject[rows,cols];

        // Fill the grid with the children of this gameobject
        int i = 0;
        int j = 0;
        foreach(Transform child in transform) {
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

        // TODO: Position the GameObjects according to the grid? Or position them in inspector?
    }

    // Navigates the UI to this folder 
    // (shows folders and files in the grid object on this script)
    // Takes a Folder argument specifying what folder the player is navigating from
    public void Navigate(Folder previous) {
        previous.SetAllActive(false);
        SetAllActive(true);
        // TODO: onclick function for a folder will call a function in Player script
        // that function will be passed in the clicked folder, and the player
        // script will call Navigate on that folder, passing in the current folder
        // then the player script will update the current folder
    }

    // Enables/disables all GameObjects in the grid
    // TODO: Instead of SetActive, might have to disable sprite so that 
    // parent objects don't override their children's visibility 
    public void SetAllActive(bool isActive) {
        foreach(GameObject file in grid) {
            file.SetActive(isActive);
        }
    }
}
