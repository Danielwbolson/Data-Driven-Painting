using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour {

    float zoom = 10;
    Camera cam;
    // Use this for initialization
    void Start () {
        cam = Camera.main;
    }
    
    // Update is called once per frame
    void Update () {
        Vector3 v = Input.mousePosition;
        v.z = zoom;
        transform.position = cam.ScreenToWorldPoint(v);

        zoom += Input.GetAxis("Mouse ScrollWheel");
    }
}
