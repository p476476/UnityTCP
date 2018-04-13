using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExampleSocket : MonoBehaviour {

    private SocketManager mSocketMgr;
    string server_ip = "127.0.0.1";
    int server_port = 5566;
    public InputField input_box;
    string username = "pohong";

    bool ready_to_disconnect = false;
    [SerializeField]
    private TextLogController textLogController;

    public void Update()
    {
        UpdateRecvMsg();

        if (ready_to_disconnect) { 
            mSocketMgr.Close();
            ready_to_disconnect = false;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            JointData[] jd = new JointData[2];
            jd[0] = new JointData(1, 2, 3, 4, 5);
            jd[1] = new JointData(1, 2, 3, 4, 5);
            string json = JsonHelper.ToJson<JointData>(jd,true);
            Debug.Log(json);
        }
    }

    public void OnSendString(string str)
    {
        
    }

    public void OnServerIPChange(string ip)
    {
        server_ip = ip;
    }

    public void OnServerPortChange(string port)
    {
        server_port = int.Parse(port);
    }

    void Start()
    {
        mSocketMgr = new SocketManager();
    }

    public void OnClickConnect()
    {
        bool state = mSocketMgr.Connect(server_ip, server_port);
        if (state == true)
        {
            textLogController.LogText("成功連接至SERVER");
            JsonData jdata = new JsonData();
            jdata.cmd = "my name is";
            jdata.name = username;
            jdata.data = "";
            mSocketMgr.SendServer(getJsonString(jdata));
        }
        else
            textLogController.LogText("無法連接至SERVER");
    }

    public void OnClickDisconnect()
    {
        JsonData jdata = new JsonData();
        jdata.cmd = "disconnect";
        jdata.name = username;
        jdata.data = "";
        mSocketMgr.SendServer(getJsonString(jdata));
        
    }

    private void UpdateRecvMsg()
    {
        while (mSocketMgr.recieveData.Count > 0)
        {
            handleRecvMsg(mSocketMgr.recieveData[0]);
            
            mSocketMgr.recieveData.Remove(mSocketMgr.recieveData[0]);
        }
    }


    private void handleRecvMsg(string str)
    {
        JsonData jdata = JsonUtility.FromJson<JsonData>(str);
        switch(jdata.cmd)
        {
            case "say":
                if(jdata.name=="track")
                {
                    Debug.Log("jdata.data=" + jdata.data);
                    string fixedData = "{\"Items\":" + jdata.data + "}";
                    JointData[] jointDatas = JsonHelper.FromJson<JointData>(fixedData);
                    /*
                     * Log 接收到的資料
                     * foreach (JointData jointData in jointDatas)
                    {
                        textLogController.LogText(jointData.getString());
                    }
                    */

                    Main.instance.udpateJointsData(jointDatas);


                }
                else
                {
                    textLogController.LogText(jdata.name + "說:" + jdata.data);
                }
                
                break;
            case "disconnect accept":
                ready_to_disconnect = true;
                break;
                    
        }
        
    }
    public void OnClickClose()
    {
        mSocketMgr.Close();
    }

    public void OnClickSend()
    {
        JsonData jdata = new JsonData();
        jdata.cmd = "say";
        jdata.name = username;
        jdata.data = input_box.text;

        mSocketMgr.SendServer(getJsonString(jdata));
        input_box.text = "";
    }

    public void OnClickGetTime()
    {
        JsonData jdata = new JsonData();
        jdata.cmd = "what time";
        jdata.name = username;
        jdata.data = "";

        mSocketMgr.SendServer(getJsonString(jdata));
    }

    private string getJsonString(JsonData jdata)
    {
        string json = JsonUtility.ToJson(jdata);
        return json;
    }
}
