using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActions : MonoBehaviour
{
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

    public int rows = 2;
    public int cols = 3;

    // [Header("Alert Boxes")]
    // public static float alertDuration;
    // public static bool isAlert;
    // public static GameObject activeAlert;
    // public static GameObject succCorrupt;
    // public static GameObject fileRestored;
    // public static GameObject failCorrupt;
    // public static GameObject succDDOS;
    // public static GameObject failDDOS;
    // public static GameObject succZip;
    // public static GameObject failZip;
    // public static GameObject recoveryDDOS;
    // public static GameObject ipWarning;
    // public static GameObject ipSwitch;
    // public static GameObject ddosAvailable;


    private bool isAtRoot = true;
    private File selectedFile;

    // private void Awake() {
    //     isAlert = false;
    //     activeAlert = null;
    //     succCorrupt = GameObject.Find("Text_SuccZip");
    //     fileRestored = GameObject.Find("Text_FileRestored");
    //     failCorrupt = GameObject.Find("Text_FailCorrupt");
    //     succDDOS = GameObject.Find("Text_SuccDDOS");
    //     failDDOS = GameObject.Find("Text_FailDDOS");
    //     succZip = GameObject.Find("Text_SuccZip");
    //     failZip = GameObject.Find("Text_FailZip");
    //     recoveryDDOS = GameObject.Find("Text_RecoveryDDOS");
    //     ipWarning = GameObject.Find("Text_IPWarning");
    //     ipSwitch = GameObject.Find("Text_IPSwitch");
    //     ddosAvailable = GameObject.Find("Text_DDOSAvailable");

    // }
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
        if (selectedFile.isVulnerable)
        {
            selectedFile.SetCorrupted(true);
            cybersec.fixFile(selectedFile);
            if (selectedFile.parent != null) selectedFile.parent.UpdateIsBombable();
            DeselectFile();
            StartCoroutine(ButtonCooldown(corruptButton, corruptButtonCooldown));
            alert.DisplayAlert(alert.succCorrupt);
            //Debug.Log("Corrupt file success");
            // iterate through all folders,
            // call folder.UpdateIsBombable()
            // if isBombable is true after, display message
        } 
        else
        {
            alert.DisplayAlert(alert.failCorrupt);
            //Debug.Log("Corrupt file failed; insufficient permissions");
            DeselectFile();
        }
        lm.RefreshLabels();
        //on corrupt File button click, corrupts whatever file is selected
        //unless player has insufficient permissions, in which case the player
        //will lose health. This will partially preoccupy the opponent (cybersec team)
        //as they attempt to restore the file, which will give the player more time to do other things
    }

    public void DDOS() {
        if (cybersec.canDDOS){
            cybersec.GetPwned();
            StartCoroutine(ButtonCooldown(ddosButton, ddosButtonCooldown));
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
        Debug.Log("Lose condition met for player, you lose =(");
    }
}
