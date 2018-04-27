using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealCamera : MonoBehaviour {

    public enum Type
    {
        rgb_camera,
        depth_camera
    }
    public Type type;
    public float fov=78;
    public float proj_width = 0.32f;
    public float proj_height = 0.24f;
    public float proj_diatence = 0.2f;
    public int pixel_width = 640;
    public int pixel_height = 480;



    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
