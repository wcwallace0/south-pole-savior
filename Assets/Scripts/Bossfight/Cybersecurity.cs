using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Cybersecurity : MonoBehaviour
{
    public Alert alert;
    public bool isPwned = false; //boolean indicating whether the enemy is currently DDOSed
    public int actionPoints;
    public int maxPoints;
    public int ipProgress; //scale is 0-4. 0 means no progress towards finding players IP, 4 means IP has been found.
    public int ipGoal;
    public PlayerActions player;
    public bool isFindIPActive = true;
    public float ipTimer;
    public float fixFileTimer;
    public bool gameOver;
    public bool fileCorrupted;
    public static List<File> corruptedFiles;
    public float ddosInactivityTime;

    public bool canDDOS;
    public float ddosTimer;

    // Start is called before the first frame update
    void Start()
    {
        actionPoints = maxPoints;
        corruptedFiles = new List<File>();
        StartCoroutine(FindIP());
        StartCoroutine(DDOSTimer(ddosTimer));
    }

    void FixedUpdate(){
        if (ipProgress == ipGoal && !gameOver){
            player.Defeat();
            gameOver = true;

        } else
        {
            if (!isFindIPActive && !gameOver)
            {
                StartCoroutine(FindIP());
            }
        }

        if (ipProgress == (ipGoal - 1)){
            alert.DisplayAlert(alert.ipWarning);
        }
    }

    //on DDOS button click, this is essentially an ultimate ability for the player
    //more than a cooldown, it will likely need to be "charged", which is a mechanic I still
    //need to figure out. If successful, the DDOS attack will disable the opponent from taking any
    //action, giving the player time to do more things.
    public void GetPwned(){
        isPwned = true;
        canDDOS = false;
        StartCoroutine(DDOSTimer(ddosTimer));
        actionPoints = 0;
        ipProgress= 0;
        StopAllCoroutines();
        StartCoroutine(DisableCybersec());
    }

    public void UnPwned(){
        isPwned = false;
        actionPoints = maxPoints;
        StartCoroutine(FindIP());
        alert.DisplayAlert(alert.recoveryDDOS);
    }

    IEnumerator DisableCybersec() {
        yield return new WaitForSeconds(ddosInactivityTime);
        UnPwned();
    }

    public void fixFile(File file){
        StartCoroutine(FixFl(file));
    }

    public void incrementAcionPts()
    {
        if(actionPoints != maxPoints) {
            actionPoints ++;
            if (actionPoints == 1){
                File fl = FindCorruptedFile();
                //check if file is good
                if(fl != null) {
                    StartCoroutine(FixFl(fl));
                }
            }
        }
    }

    private File FindCorruptedFile() {
        if(corruptedFiles.Any()) {
            return corruptedFiles[0];
        } else {
            return null;
        }
    }

    IEnumerator FixFl(File fl)
    {
        if (actionPoints > 0) {
            actionPoints --;
            yield return new WaitForSeconds(fixFileTimer);
            actionPoints ++;
            fl.SetCorrupted(false);
            alert.DisplayAlert(alert.fileRestored);
        }

    }

    IEnumerator FindIP()
    {
        //Debug.Log("FindIP coroutine started");
        isFindIPActive = true;
        yield return new WaitForSeconds(ipTimer);
        ipProgress ++;
        isFindIPActive = false;
        //Debug.Log("FindIP coroutine ended, ipProgress is " +ipProgress + " out of 4");
    }

    IEnumerator DDOSTimer(float timer){
        yield return new WaitForSeconds(timer);
        canDDOS = true;
    }
    public void KillCoroutines()
    {
        StopAllCoroutines();
    }
}
