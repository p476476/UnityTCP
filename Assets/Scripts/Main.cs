using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

    public static Main instance;
    public const int camera_count=1;

    public List<GameObject> human_list;
    public GameObject human_prefeb;

    public Dictionary<string, int> track_data_index = new Dictionary<string, int>();

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

        track_data_index.Add("track00",0);
        track_data_index.Add("track01", 1);
        track_data_index.Add("track02", 2);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void udpateJointsData(int camera_number,JointData[] jointsData)
    {

        human_list[0].GetComponent<Human>().udpateJointsData2D(camera_number, jointsData);
        udpateHumanPose();
    }

    public void udpateHumanPose()
    {
        human_list[0].GetComponent<Human>().udpateHumanPose();
    }
}
