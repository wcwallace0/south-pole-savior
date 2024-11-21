using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Cybersecurity : MonoBehaviour
{
    public bool isPwned = false; //boolean indicating whether the enemy is currently DDOSed
    public int actionPoints;
    public int maxPoints = 4;
    public int ipProgress; //scale is 0-4. 0 means no progress towards finding players IP, 4 means IP has been found.
    public PlayerActions player;
    public bool isFindIPActive = true;
    public float ipTimer;
    public float fixFileTimer;
    public bool gameOver;
    public bool fileCorrupted;
    public static List<File> corruptedFiles;
    public float ddosCooldown;
    public float ddosButtonCooldown;
    public Button ddosButton;

    // Start is called before the first frame update
    void Start()
    {
        actionPoints = maxPoints;
        corruptedFiles = new List<File>();
        StartCoroutine(FindIP());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate(){
        if (ipProgress == maxPoints && !gameOver){
            player.Defeat();
            gameOver = true;

        } else
        {
            if (!isFindIPActive && !gameOver)
            {
                StartCoroutine(FindIP());
            }
        }
    }

    public void GetPwned(){
        isPwned = true;
        actionPoints = 0;
        ipProgress= 0;
        StopAllCoroutines();
        ddosButton.interactable = false;
        StartCoroutine(DDOSCooldown());
        StartCoroutine(DDOSButtonCooldown());
    }

    public void UnPwned(){
        isPwned = false;
        actionPoints = maxPoints;
        StartCoroutine(FindIP());
    }

    IEnumerator DDOSCooldown() {
        yield return new WaitForSeconds(ddosCooldown);
        UnPwned();
    }

    IEnumerator DDOSButtonCooldown() {
        yield return new WaitForSeconds(ddosButtonCooldown);
        ddosButton.interactable = true;
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
        }

    }

    IEnumerator FindIP()
    {
        Debug.Log("FindIP coroutine started");
        isFindIPActive = true;
        yield return new WaitForSeconds(ipTimer);
        ipProgress ++;
        isFindIPActive = false;
        Debug.Log("FindIP coroutine ended, ipProgress is " +ipProgress + " out of 4");
    }
}
