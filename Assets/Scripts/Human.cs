using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Human : MonoBehaviour {

    public BoneDrawer[] bone_drawer;


    //2D joint data(pixel)        [cam1,cam2,cam3]
    Skeleton2D[] skeleton2D ;
    int joint_count=14;

    //3D joint transform         [head, neck, Rsho, Relb, Rwri, Lsho, Lelb, Lwri, Rhip, Rkne, Rank, Lhip, Lkne, Lank]
    public Transform[] joints3D_transform;
    public Transform skeleton3D;

    public Transform[] joints2D_transform0;
    bool useDepth0 = false;
    public Transform[] joints2D_transform1;
    bool useDepth1 = true;

    //limb in unity.         [[neck,head],[neck, Rsho] ......]
    int[,] limbs_index = new int[,] { { 1, 0 }, { 1, 2 }, { 2, 3 }, { 3, 4 }, { 1, 5 }, { 5, 6 }, { 6, 7 }, { 1, 8 }, { 8, 9 }, { 9, 10 }, { 1, 11 }, { 11, 12 }, { 12, 13 } };
    float depth_alpha = 0.5f;

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

   public void setDepthScale(float f)
    {
        skeleton2D[1].depth_scale = f;
    }

    public void updateJointsData2D(int camera_number,JointData[] jointsData)
    {
        bool[] is_recv = new bool[14];
        Vector3[] pos_data = new Vector3[14];
        int[] ref_joint = new int[] { 1, -1, 1, 2, 3, 1, 5, 6, 1, 8, 9, 1, 11, 12 };

        //convert json data to position array
        for (int i = 0; i < jointsData.Length; i++)
        {
            int j_num = (int)jointsData[i].jn;
            if (j_num <= 13)
            {
                pos_data[j_num].Set(jointsData[i].x, jointsData[i].y, 0);
                is_recv[j_num] = true;
            }
        }

        //check if get root pos(nect) and calculate disp
        Vector3[] disp = new Vector3[14];
        if (is_recv[1] == false)
            return;
        else
        {
            disp[1] = pos_data[1]- skeleton2D[camera_number].joint_position[1];
            skeleton2D[camera_number].joint_position[1] = pos_data[1];
        }

        //update each joint position
        for (int i = 0; i < pos_data.Length; i++)
        {
            if (i == 1)
                continue;
            if (is_recv[i] == false) {
                skeleton2D[camera_number].joint_position[i] += disp[ref_joint[i]];
                disp[i] = disp[ref_joint[i]];
            }
            else
            {
                disp[i] = pos_data[i] - skeleton2D[camera_number].joint_position[i];
                skeleton2D[camera_number].joint_position[i] = pos_data[i];
            }
        }


    }

    public void updateDepthData(int camera_number, JointDepthData[] depth_data)
    {

        bool[] is_recv = new bool[14];
        float[] dp_array = new float[14];
        int[] ref_joint = new int[] { 1, -1, 1, 2, 3, 1, 5, 6, 1, 8, 9, 1, 11, 12 };

        //convert json data to position array
        for (int i = 0; i < depth_data.Length; i++)
        {
            int j_num = (int)depth_data[i].jn;
            if (j_num <= 13 && depth_data[i].dp > skeleton2D[camera_number].depth_min_bound && depth_data[i].dp < skeleton2D[camera_number].depth_max_bound)
            {
                dp_array[j_num] = depth_data[i].dp* skeleton2D[camera_number].depth_scale;
                is_recv[j_num] = true;
            }
        }

        //check if get root pos(nect) and calculate disp
        float[] disp = new float[14];

        if (is_recv[1] == false)
            return;
        else
        {
            disp[1] = dp_array[1] - skeleton2D[camera_number].depth_data[1];
            skeleton2D[camera_number].depth_data[1] += disp[1]*depth_alpha;
        }

        //update each joint depth
        for (int i = 0; i < dp_array.Length; i++)
        {
            if (i == 1)
                continue;
            if (is_recv[i] == false)
            {
                skeleton2D[camera_number].depth_data[i] += disp[ref_joint[i]] * depth_alpha;
                disp[i] = disp[ref_joint[i]];
            }
            else
            {
                disp[i] = dp_array[i] - skeleton2D[camera_number].depth_data[i];
                skeleton2D[camera_number].depth_data[i] += disp[i] * depth_alpha;
            }
        }

        updateHumanPose3D();
    }

    public void updateHumanPose2D()
    {
        Vector3[] nor_joints_position = skeleton2D[0].getNormalizeJointsPosition(); //(z=0)
        float radio=1.33f;

        for (int i = 0; i < nor_joints_position.Length; i++)
        {
            joints2D_transform0[i].localPosition = new Vector3(nor_joints_position[i].x*radio, nor_joints_position[i].y,0);
        }
        if(useDepth0)
        {
            for (int i = 0; i < skeleton2D[0].depth_data.Length; i++)
            { 
                joints2D_transform0[i].localPosition = new Vector3(joints2D_transform0[i].localPosition.x, joints2D_transform0[i].localPosition.y, skeleton2D[0].depth_data[i]);
            }
        }

        Vector3[] nor_joints_position1 = skeleton2D[1].getNormalizeJointsPosition(); //(z=0)

        for (int i = 0; i < nor_joints_position1.Length; i++)
        {
            joints2D_transform1[i].localPosition = nor_joints_position1[i];
        }
        if (useDepth1)
        {
            for (int i = 0; i < skeleton2D[1].depth_data.Length; i++)
            {
                joints2D_transform1[i].localPosition = new Vector3(joints2D_transform1[i].localPosition.x, joints2D_transform1[i].localPosition.y, skeleton2D[1].depth_data[i]);
            }
        }
    }


    public void updateHumanPose3D()
    {
        int camera_count = Main.instance.cameras.Length;
        //score confidence for each 2d joint
        float[,] confidence = new float[camera_count, joint_count];
        Vector3[] cameras_pos = new Vector3[camera_count];
        Vector3[] cameras_dir = new Vector3[camera_count];




        //detect occlusiion
        for(int i = 0; i < confidence.GetLength(0); i++)
        {
            for(int j=1;j<confidence.GetLength(1);j++)
            {
                
            }
        }



        //only use high confidence 2d joint to fuse the 3d human skeleton model
        for (int i = 0; i < joints3D_transform.Length; i++)
        {
            joints3D_transform[i].localPosition = new Vector3(joints2D_transform0[i].localPosition.x, joints2D_transform1[i].localPosition.y, joints2D_transform1[i].localPosition.x);
        }


    }
}
