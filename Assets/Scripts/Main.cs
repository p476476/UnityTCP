using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

    public static Main instance;
    List<Human> human_list;

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
        human_list = new List<Human>();
        Human human = new Human();
        human_list.Add(human);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void udpateJointsData(JointData[] jointsData)
    {
        human_list[0].udpateJointsData2D(jointsData);
    }

    public void udpateHumanPose()
    {
        human_list[0].udpateHumanPose();
    }
}
