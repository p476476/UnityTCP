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

    public Text uitxt_sever_ip;
    public Text uitxt_sever_port;
    public Text uitxt_username;

    bool ready_to_disconnect = false;
    [SerializeField]
    private TextLogController textLogController;

    
    

    public void Start()
    {
        server_ip = uitxt_sever_ip.text;
        server_port = int.Parse(uitxt_sever_port.text);
        mSocketMgr = new SocketManager();
    }

    public void Update()
    {
        UpdateRecvMsg();

        if (ready_to_disconnect) { 
            mSocketMgr.Close();
            ready_to_disconnect = false;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            JsonData jdata = new JsonData();
            jdata.cmd = "add group";
            jdata.name = username;
            jdata.data = "get track data";
            mSocketMgr.SendServer(getJsonString(jdata));
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


    public void OnClickConnect()
    {
        print(server_ip);
        print(server_port);
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
        //spilit cmds
        List<string> strs = new List<string>();
        while (true)
        {
            int multi_str = str.IndexOf("{\"cmd\":", 9);
            if (multi_str != -1)
            {
                strs.Add(str.Substring(0, multi_str));
                str = str.Substring(multi_str);
            }else
            {
                strs.Add(str);
                break;
            }
        }

        //each string do
        foreach (string s in strs)
        {
            JsonData jdata;
            print("JOSN:"+s);
            try
            {
                jdata = JsonUtility.FromJson<JsonData>(s);
            }
            catch
            {
                print("ERROR Json input");
                continue;
            }

                

            switch (jdata.cmd)
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
                        textLogController.LogText("Undefine tracker:" + jdata.name);
                    }
                    break;
                case "disconnect accept":
                    ready_to_disconnect = true;
                    break;

            }
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
