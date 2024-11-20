using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cybersecurity : MonoBehaviour
{
    public bool isPwned = false; //boolean indicating whether the enemy is currently DDOSed
    public int actionPoints;
    public int ipProgress; //scale is 0-4. 0 means no progress towards finding players IP, 4 means IP has been found.
    public PlayerActions player;
    public bool isFindIPActive = true;
    public float ipTimer;
    public float fixFileTimer;
    public bool gameOver;
    public bool fileCorrupted;
    public Folder rootFolder;

    // Start is called before the first frame update
    void Start()
    {
        actionPoints = 4;
        StartCoroutine(FindIP());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate(){
        if (ipProgress == 4 && !gameOver){
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
    }

    public void UnPwned(){
        isPwned = false;
        actionPoints = 4;
    }

    public void fixFile(File file){
        if (actionPoints > 0) {
           actionPoints --;
           StartCoroutine(FixFl(file));
        }
    }

    public void incrementAcionPts()
    {
        if(actionPoints != 4) {
            actionPoints ++;
            if (actionPoints == 0){
                File fl = FindCorruptedFile();
                //check if file is good
                fixFile(fl);
            }
        }
    }

    private File FindCorruptedFile() {
        // search file system for any corrupted file, return first one
        return null;
    }

    IEnumerator FixFl(File fl)
    {
        yield return new WaitForSeconds(fixFileTimer);
        actionPoints ++;
        fl.SetCorrupted(false);

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
