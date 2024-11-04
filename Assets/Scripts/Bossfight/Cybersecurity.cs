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
        if (ipProgress == 4){
            player.Defeat();

        } else
        {
            if (!isFindIPActive)
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

    IEnumerator FindIP()
    {
        Debug.Log("FindIP coroutine started");
        isFindIPActive = true;
        yield return new WaitForSeconds(10);
        ipProgress ++;
        isFindIPActive = false;
        Debug.Log("FindIP coroutine ended, ipProgress is " +ipProgress + " out of 4");
    }
}
