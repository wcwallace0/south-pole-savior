using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActions : MonoBehaviour
{
    public Folder currentFolder;
    public int playerHealth;
    public Cybersecurity cybersec;

    [Header("Buttons")]
    public Button zipButton;
    public float zipButtonCooldown;
    public Button corruptButton;
    public float corruptButtonCooldown;
    public Button ddosButton;
    public float ddosButtonCooldown;
    public Button ipButton;
    public float ipButtonCooldown;

    private bool isAtRoot = true;
    private File selectedFile;

    public void NavigateFolder(Folder newFolder) {
        DeselectFile();
        newFolder.Navigate(currentFolder);
        currentFolder = newFolder;
        isAtRoot = false;
    }

    public void NavigateBack() {
        if(!isAtRoot) {
            DeselectFile();
            Folder parent = currentFolder.transform.parent.GetComponent<Folder>();
            parent.Navigate(currentFolder);
            currentFolder = parent;

            if(parent.isRoot) {
                isAtRoot = true;
            }
        }
    }

    public void SelectFile(File fl) {
        selectedFile = fl;
        selectedFile.SetSelected(true);
        corruptButton.interactable = true;
    }

    public void DeselectFile() {
        if(selectedFile != null) {
            selectedFile.SetSelected(false);
            selectedFile = null;
            corruptButton.interactable = false;
        }
    }

    public void ZipBomb()
    {
        if (currentFolder.isBombable)
        {
            //if (folder is one of the top level folders)
            //{
                //activate fight phase two
            //} else
            //{
                currentFolder.Corrupt();
                NavigateBack();
                StartCoroutine(ButtonCooldown(zipButton, zipButtonCooldown));
                Debug.Log("ZIP Bomb success, folder corrupted.");
                //corrupt all files in the folder
            //}
        } else
        {
            Debug.Log("ZIP Bomb failed; insufficient file upload permissions");
        }

        //on ZIP Bomb button click, instantly corrupts whatever folder
        //the player is currently in if certain prereqs are met, otherwise
        //it will fail and player will lose health. a successful use of the zip bomb
        //button in the top level folder will be a win condition for the player.

    }

    public void CorruptFile()
    {      
        if (selectedFile.isVulnerable)
        {
            selectedFile.SetCorrupted(true);
            cybersec.fixFile(selectedFile);
            DeselectFile();
            StartCoroutine(ButtonCooldown(corruptButton, corruptButtonCooldown));
            if (selectedFile.parent != null) selectedFile.parent.UpdateIsBombable();
            
            // iterate through all folders,
            // call folder.UpdateIsBombable()
            // if isBombable is true after, display message
        } 
        else
        {
            Debug.Log("Corrupt file failed; insufficient permissions");
        }

        //on corrupt File button click, corrupts whatever file is selected
        //unless player has insufficient permissions, in which case the player
        //will lose health. This will partially preoccupy the opponent (cybersec team)
        //as they attempt to restore the file, which will give the player more time to do other things
    }

    public void DDOS() {
        cybersec.GetPwned();
        StartCoroutine(ButtonCooldown(ddosButton, ddosButtonCooldown));
    }

    //on IP switch button click, this is a defensive move for the player. there might be a constant
    //timer where the player will be locked out of certain folders unless they switch their IP,
    //and possibly a constant timer which tracks how long it's been since the player switched IPs.
    //if that timer expires the player will be locked out of the system and lose automatically.
    public void IpSwitch()
    {
        cybersec.ipProgress = 0;
        StartCoroutine(ButtonCooldown(ipButton, ipButtonCooldown));
    }

    IEnumerator ButtonCooldown(Button button, float cooldown) {
        button.interactable = false;
        yield return new WaitForSeconds(cooldown);
        button.interactable = true;
    }

    public void Defeat()
    {
        Debug.Log("Lose condition met for player, you lose =(");
    }
}
