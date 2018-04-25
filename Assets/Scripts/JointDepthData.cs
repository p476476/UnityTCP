using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class JointDepthData  {
    public float jn;
    public float dp;

    public JointDepthData(float jn, float dp)
    {
        this.jn = jn;
        this.dp = dp;
    }

    public string getString()
    {
        return string.Format("Joint {0}'s depth:{1})", jn, dp);
    }
}
