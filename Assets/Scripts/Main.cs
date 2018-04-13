using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

    public static Main instance;
    List<GameObject> human_list;
    public GameObject human_prefeb;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }else
        {
            Debug.Log("there are more than one 'main' instance.");
        }
    }

    // Use this for initialization
    void Start () {

        human_list = new List<GameObject>();
        //assume only one human
        human_list.Add(Instantiate(human_prefeb)as GameObject);

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void udpateJointsData(JointData[] jointsData)
    {
        human_list[0].GetComponent<Human>().udpateJointsData2D(jointsData);
        udpateHumanPose();
    }

    public void udpateHumanPose()
    {
        human_list[0].GetComponent<Human>().udpateHumanPose();
    }
}
