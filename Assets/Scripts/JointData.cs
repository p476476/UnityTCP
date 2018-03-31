using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class JointData  {
    public float joint_number;
    public float x0;
    public float y0;
    public float x1;
    public float y1;

    public JointData(float jn,float x0,float y0,float x1,float y1)
    {
        this.joint_number = jn;
        this.x0 = x0;
        this.y0 = y0;
        this.x1 = x1;
        this.y1 = y1;
    }

    public string getString()
    {
        return string.Format("Joint{0}: From ({1},{2}) to ({3},{4}).", joint_number, x0, y0, x1, y1);
    }
}
