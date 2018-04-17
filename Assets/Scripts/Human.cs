using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Human : MonoBehaviour {

    public BoneDrawer[] bone_drawer;


    //2D joint data(pixel)        [cam1,cam2,cam3]
    Skeleton2D[] skeleton2D ;


    //3D joint transform         [head, neck, Rsho, Relb, Rwri, Lsho, Lelb, Lwri, Rhip, Rkne, Rank, Lhip, Lkne, Lank]
    public Transform[] joints_transform;

    public Transform[] joints2D_transform0;
    public Transform[] joints2D_transform1;

    //limb in unity.         [[neck,head],[neck, Rsho] ......]
    int[,] limbs_index = new int[,] { { 1, 0 }, { 1, 2 }, { 2, 3 }, { 3, 4 }, { 1, 5 }, { 5, 6 }, { 6, 7 }, { 1, 8 }, { 8, 9 }, { 9, 10 }, { 1, 11 }, { 11, 12 }, { 12, 13 } };


    // Use this for initialization
    void Start () {

        skeleton2D = new Skeleton2D[3];
        //假設最多3台camera 產生3組2D骨架
        for (int i = 0; i < 3; i++)
        {

            skeleton2D[i] = new Skeleton2D();
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void udpateJointsData2D(int camera_number,JointData[] jointsData)
    {
        for (int i = 0; i < jointsData.Length; i++)
        {
            int j_num = (int)jointsData[i].jn;
            if (j_num <= 13)
            {
                skeleton2D[camera_number].joint_position[j_num].Set(jointsData[i].x, jointsData[i].y, 0);
            }
        }


    }

    public void udpateHumanPose()
    {
        Vector3[] nor_joints_position = skeleton2D[0].getNormalizeJointsPosition(); //(z=0)

        for (int i = 0; i < nor_joints_position.Length; i++)
        {
            joints2D_transform0[i].localPosition = nor_joints_position[i];
        }

        Vector3[] nor_joints_position1 = skeleton2D[1].getNormalizeJointsPosition(); //(z=0)

        for (int i = 0; i < nor_joints_position1.Length; i++)
        {
            joints2D_transform1[i].localPosition = nor_joints_position1[i];
        }
    }
}
