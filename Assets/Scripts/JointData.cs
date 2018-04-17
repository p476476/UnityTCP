using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class JointData  {
    public float jn;
    public float x;
    public float y;


    public JointData(float jn,float x,float y)
    {
        this.jn = jn;
        this.x = x;
        this.y = y;
    }

    public string getString()
    {
        return string.Format("Joint{0}:({1},{2})", jn, x, y);
    }
}
