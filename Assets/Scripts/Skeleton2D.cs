using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Skeleton2D {
    //儲存Python傳送的2D骨架資訊(in pixel)
    public Vector3[] joint_position; //in pixel
    public Skeleton2D ()
    {
        joint_position = new Vector3[14];
    }
    public Vector3[] getNormalizeJointsPosition()
    {
        Vector3[] nor_joint_position = joint_position.Clone() as Vector3[];

        for(int i=0;i<joint_position.Length;i++)
        {

            nor_joint_position[i].x /= 432f;
            nor_joint_position[i].y = 1-(nor_joint_position[i].y/368f);//顛倒

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

    
}
