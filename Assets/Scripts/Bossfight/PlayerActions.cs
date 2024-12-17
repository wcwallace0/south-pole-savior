using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerActions : MonoBehaviour
{
    public LoadGame loader;
    public Folder currentFolder;
    public Alert alert;
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
    public LabelManager lm;


    private bool isAtRoot = true;
    private File selectedFile;

    void Start()
    {
        lm = FindObjectOfType<LabelManager>();
    }
    
    public void NavigateFolder(Folder newFolder) {
        DeselectFile();
        newFolder.Navigate(currentFolder);
        currentFolder = newFolder;
        isAtRoot = false;
        lm.RefreshLabels();
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
        lm.RefreshLabels();
    }

    public void SelectFile(File fl) {
        DeselectFile();
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
                alert.DisplayAlert(alert.succZip);
                //Debug.Log("ZIP Bomb success, folder corrupted.");
                //corrupt all files in the folder
            //}
        } else
        {
            alert.DisplayAlert(alert.failZip);
            //Debug.Log("ZIP Bomb failed; insufficient file upload permissions");
        }

        //on ZIP Bomb button click, instantly corrupts whatever folder
        //the player is currently in if certain prereqs are met, otherwise
        //it will fail and player will lose health. a successful use of the zip bomb
        //button in the top level folder will be a win condition for the player.
        lm.RefreshLabels();
    }

    public void CorruptFile()
    {
        if(selectedFile.name == "System32") { 
            alert.DisplayAlert(alert.niceTry);
            selectedFile.isVulnerable = false;
        }
        if (selectedFile.isVulnerable)
        {
            selectedFile.SetCorrupted(true);
            cybersec.fixFile(selectedFile);
            if (selectedFile.parent != null) selectedFile.parent.UpdateIsBombable();
            // if(selectedFile.name == "CLASSIFIED.exe"){
            //     alert.DisplayAlert(alert.DDOSbuff);
            //     ddosButtonCooldown --;
            //     cybersec.ddosTimer -= 2;
            // } else { 
            alert.DisplayAlert(alert.succCorrupt); 
            //}
            DeselectFile();
            StartCoroutine(ButtonCooldown(corruptButton, corruptButtonCooldown));
        } 
        else
        {
            alert.DisplayAlert(alert.failCorrupt);
            DeselectFile();
        }
        lm.RefreshLabels();
    }

    public void DDOS() {
        if (cybersec.canDDOS){
            cybersec.GetPwned();
            //StartCoroutine(ButtonCooldown(ddosButton, ddosButtonCooldown));
            alert.DisplayAlert(alert.succDDOS);
        } else{
            alert.DisplayAlert(alert.failDDOS);
        }
    }

    //on IP switch button click, this is a defensive move for the player. there might be a constant
    //timer where the player will be locked out of certain folders unless they switch their IP,
    //and possibly a constant timer which tracks how long it's been since the player switched IPs.
    //if that timer expires the player will be locked out of the system and lose automatically.
    public void IpSwitch()
    {
        cybersec.ipProgress = 0;
        StartCoroutine(ButtonCooldown(ipButton, ipButtonCooldown));
        alert.DisplayAlert(alert.ipSwitch);
    }

    IEnumerator ButtonCooldown(Button button, float cooldown) {
        button.interactable = false;
        yield return new WaitForSeconds(cooldown);
        button.interactable = true;
    }

    public void Defeat()
    {
        loader.EndGame(false);
    }

    public void KillCoroutines()
    {
        StopAllCoroutines();
    }
}
