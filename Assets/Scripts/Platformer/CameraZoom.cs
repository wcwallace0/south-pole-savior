using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public float minZoom;
    public float maxZoom;
    public float zoomThreshold; // Camera will start zooming out once player velocity reaches this
    public float zoomSpeed;
    public PlayerMovement pm; 
    
    private Camera cam; // This object's camera component
    private float zoomRange;
    private float velRange;

    private void Start() {
        zoomRange = maxZoom - minZoom;
        velRange = pm.maxVelocity - zoomThreshold;
        cam = GetComponent<Camera>();
    }

    void Update() {
        SetZoom();
    }

    private void SetZoom() {
        float vel = Math.Abs(pm.rb.velocity.x);
        float newZoom;
        if(vel < zoomThreshold) {
            newZoom = minZoom;
        } else if(vel >= pm.maxVelocity) {
            newZoom = maxZoom;
        } else {
            // find percent of vel between zoomThreshold and maxVelocity
            // set zoom to that percent between minZoom and maxZoom
            float p = (vel-zoomThreshold) / velRange;
            newZoom = minZoom + (zoomRange * p);
        }

        // Move towards newZoom
        cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, newZoom, zoomSpeed);
    }
}
