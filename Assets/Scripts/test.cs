using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        string str = "123456789123456789123456789123456789123456789";
        List<string> strs = new List<string>();
        while (true)
        {
            int multi_str = str.IndexOf("12", 3);
            print("m="+multi_str);
            if (multi_str != -1)
            {
                strs.Add(str.Substring(0, multi_str ));
                str = str.Substring(multi_str);
            }
            else
            {
                strs.Add(str);
                break;
            }
        }
        foreach(string s in strs)
        {
            print(s);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
