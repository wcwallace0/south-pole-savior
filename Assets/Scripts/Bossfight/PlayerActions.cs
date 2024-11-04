using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    public Folder currentFolder;
    public File selectedFile;
    public int playerHealth;
    public Cybersecurity cybersec;

    public void NavigateFolder(Folder newFolder) {
        newFolder.Navigate(currentFolder);
        currentFolder = newFolder;
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
                currentFolder.grid = new GameObject[currentFolder.rows,currentFolder.cols];
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

    public void DeleteFile()
    {
        if (selectedFile.isVulnerable)
        {
            selectedFile.isDeleted = true;
            selectedFile.image.enabled = false;
            //make file sprite disappear

        } else
        {
            Debug.Log("Delete file failed; insufficient permissions");
        }

        //on Delete File button click, deletes whatever file is selected
        //unless player has insufficient permissions, in which case the player
        //will lose health. This will partially preoccupy the opponent (cybersec team)
        //as they attempt to restore the file, which will give the player more time to do other things
    }

    public void Ddos()
    {
        cybersec.GetPwned();

        //on DDOS button click, this is essentially an ultimate ability for the player
        //more than a cooldown, it will likely need to be "charged", which is a mechanic I still
        //need to figure out. If successful, the DDOS attack will disable the opponent from taking any
        //action, giving the player time to do more things.
    } 

    public void IpSwitch()
    {
        //on IP switch button click, this is a defensive move for the player. there might be a constant
        //timer where the player will be locked out of certain folders unless they switch their IP,
        //and possibly a constant timer which tracks how long it's been since the player switched IPs.
        //if that timer expires the player will be locked out of the system and lose automatically.
    }

    public void Defeat()
    {
        Debug.Log("Lose condition met for player, you lose =(");


    }
}
