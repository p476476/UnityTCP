using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Skeleton2D {
    //儲存Python傳送的2D骨架資訊(in pixel)
    public Vector3[] joint_position; //in pixel
    public float[] depth_data;       //in pixel 
    public float depth_scale = 0.0001f;
    public float depth_min_bound = 500;
    public float depth_max_bound = 3000;

    public Skeleton2D ()
    {
        joint_position = new Vector3[14];
        depth_data = new float[14];
    }
    public Vector3[] getNormalizeJointsPosition()
    {
        Vector3[] nor_joint_position = joint_position.Clone() as Vector3[];

        for(int i=0;i<joint_position.Length;i++)
        {
            nor_joint_position[i].x /= 640f;
            nor_joint_position[i].y = 1-(nor_joint_position[i].y/480f);//顛倒
        }
        return nor_joint_position;
    }

    public void udpateData(JointData[] jointData)
    {
        //joint id in open pose. [nose, neck, Rsho, Relb, Rwri, Lsho, Lelb, Lwri, Rhip, Rkne, Rank, Lhip, Lkne, Lank, Leye, Reye, Lear, Rear, pt19]
        //joint id in unity.     [head, neck, Rsho, Relb, Rwri, Lsho, Lelb, Lwri, Rhip, Rkne, Rank, Lhip, Lkne, Lank]
       

        for (int i=0;i<jointData.Length;i++)
        {
            int j_num = (int)jointData[i].jn;
            if (j_num <= 13)
            {
                joint_position[j_num].Set(jointData[i].x, jointData[i].y, 0);
            }
        }
    }

    public void udpateDepthData(JointDepthData[] jointDepthData)
    {
        //joint id in open pose. [nose, neck, Rsho, Relb, Rwri, Lsho, Lelb, Lwri, Rhip, Rkne, Rank, Lhip, Lkne, Lank, Leye, Reye, Lear, Rear, pt19]
        //joint id in unity.     [head, neck, Rsho, Relb, Rwri, Lsho, Lelb, Lwri, Rhip, Rkne, Rank, Lhip, Lkne, Lank]


        for (int i = 0; i < jointDepthData.Length; i++)
        {
            int j_num = (int)jointDepthData[i].jn;
            if (j_num <= 13 && jointDepthData[i].dp>depth_min_bound && jointDepthData[i].dp < depth_max_bound)
            {
                depth_data[j_num] = jointDepthData[i].dp* depth_scale;
            }
        }
    }


}
