using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneDrawer : MonoBehaviour {

    public Transform[] start_trans;
    public Transform[] end_trans;
    public LineRenderer[] lines;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        update_drawer();

    }

    public void update_drawer()
    {
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i].SetPosition(0, start_trans[i].position);
            lines[i].SetPosition(1, end_trans[i].position);
        }
    }
}
