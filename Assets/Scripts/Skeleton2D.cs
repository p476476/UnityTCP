using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton2D {
    
    public Vector3[] joint_position { get; set; } //in pixel

    public Vector3[] getNormalizeJointsPosition()
    {
        Vector3[] nor_joint_position = joint_position.Clone() as Vector3[];

        for(int i=0;i<joint_position.Length;i++)
        {
            nor_joint_position[i].x /= 640;
            nor_joint_position[i].y /= 480;
        }

        return nor_joint_position;
    }

    public void udpateData(JointData[] jointData)
    {
        //joint id in open pose. [nose, neck, Rsho, Relb, Rwri, Lsho, Lelb, Lwri, Rhip, Rkne, Rank, Lhip, Lkne, Lank, Leye, Reye, Lear, Rear, pt19]
        //joint id in unity.     [head, neck, Rsho, Relb, Rwri, Lsho, Lelb, Lwri, Rhip, Rkne, Rank, Lhip, Lkne, Lank]
       

        for (int i=0;i<jointData.Length;i++)
        {
            int j_num = (int)jointData[i].joint_number;
            if (j_num <= 13)
            {
                joint_position[j_num].Set(joint_position[j_num].x, joint_position[j_num].y, 0);
            }
        }
    }

    
}
