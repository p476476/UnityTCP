using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour {
    public Skeleton2D skeleton2D;
    BoneDrawer bone_drawer;

    //joint transform         [head, neck, Rsho, Relb, Rwri, Lsho, Lelb, Lwri, Rhip, Rkne, Rank, Lhip, Lkne, Lank]
    public Transform[] joints_transform;
    //limb in unity.         [[neck,head],[neck] Rsho, Relb, Rwri, Lsho, Lelb, Lwri, Rhip, Rkne, Rank, Lhip, Lkne, Lank]
    int[,] limbs_index = new int[,] { { 1, 0 }, { 1, 2 }, { 2, 3 }, { 3, 4 }, { 1, 5 }, { 5, 6 }, { 6, 7 }, { 1, 8 }, { 8, 9 }, { 9, 10 }, { 1, 11 }, { 11, 12 }, { 12, 13 } };


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
        Vector3[] nor_joints_position = skeleton2D.getNormalizeJointsPosition(); //(z=0)

        for (int i = 0; i < nor_joints_position.Length; i++) {
            joints_transform[i].localPosition = nor_joints_position[i];
        }
    }
}
