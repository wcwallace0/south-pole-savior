using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alert : MonoBehaviour
{
    public bool isAlert;
    public GameObject activeAlert;
    public float alertDuration;
    public GameObject succCorrupt;
    public GameObject fileRestored;
    public GameObject failCorrupt;
    public GameObject succDDOS;
    public GameObject failDDOS;
    public GameObject succZip;
    public GameObject failZip;
    public GameObject recoveryDDOS;
    public GameObject ipWarning;
    public GameObject ipSwitch;
    public GameObject ddosAvailable;
    // Start is called before the first frame update
    
    public void DisplayAlert(GameObject alert) {
        //Debug.Log("DisplayAlert called");
        if(isAlert){
            activeAlert.SetActive(false);
        }
        activeAlert = alert;
        isAlert = true;
        StartCoroutine(AlertCooldown(alert));
    }
    IEnumerator AlertCooldown(GameObject alert) {
        //Debug.Log("AlertCooldown called");
        alert.SetActive(true);
        yield return new WaitForSeconds(alertDuration);
        alert.SetActive(false);
        isAlert = false;
        activeAlert = null;
    }

    public void Test() {
        //Debug.Log("Test function in Alert class called");
    }
}
