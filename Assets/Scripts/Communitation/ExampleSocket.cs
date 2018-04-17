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
                textLogController.LogText(jdata.name + "說:" + jdata.data);
                break;
            case "track":
                if (jdata.name == "track00" || jdata.name == "track01")
                {
                    string fixedData = "{\"Items\":" + jdata.data + "}";
                    JointData[] jointDatas = JsonHelper.FromJson<JointData>(fixedData);

                    textLogController.LogText(jdata.name + "更新DATA");

                    Main.instance.udpateJointsData(Main.instance.track_data_index[jdata.name], jointDatas);
                }
                else
                {
                    textLogController.LogText("Undefine tracker:"+jdata.name );
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
