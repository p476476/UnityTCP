using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour {
    public Skeleton2D skeleton2D;
    BoneDrawer bone_drawer;
    // Use this for initialization
    void Start () {
        bone_drawer = this.GetComponent<BoneDrawer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void udpateJointsData2D(JointData[] jointsData)
    {
        skeleton2D.udpateData(jointsData);
    }

    public void udpateHumanPose()
    {
        Vector3[] nor_joints_position = skeleton2D.getNormalizeJointsPosition();
        for (int i = 0; i < nor_joints_position.Length; i++) {
            bone_drawer.start_trans[i].position = nor_joints_position[i];
            bone_drawer.end_trans[].position = nor_joints_position[];
        }

    }
}
