﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using UnityEngine.UI;


public class SocketManager {

    private Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    private byte[] _recieveBuffer = new byte[16384];
    public List<string> recieveData = new List<string>();

    /// 
    /// 建立 Connect Server.
    /// 
    public bool Connect(string IP, int Port)
    {
        try
        {

            _clientSocket.Connect(new IPEndPoint(IPAddress.Parse(IP), Port));
        }
        catch (SocketException ex)
        {
            Debug.Log(ex.Message);
            return false;
        }
        return true;
    }
    /// 
    /// 發送到 Server & 啟動接收
    /// 
    public void SendServer(String sJson)
    {
        try
        {
            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(sJson);
            SendData(byteArray);
        }
        catch (SocketException ex)
        {
            Debug.LogWarning(ex.Message);
        }
        _clientSocket.BeginReceive(_recieveBuffer, 0, _recieveBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), null);
    }

    /// 
    /// 發送封包到 Socket Server 
    /// 
    private void SendData(byte[] data)
    {
        SocketAsyncEventArgs socketAsyncData = new SocketAsyncEventArgs();
        socketAsyncData.SetBuffer(data, 0, data.Length);
        _clientSocket.SendAsync(socketAsyncData);
    }
    /// 
    /// 接收封包.
    /// 
    private void ReceiveCallback(IAsyncResult AR)
    {
        int recieved = _clientSocket.EndReceive(AR);

        Debug.Log("ReceiveCallback - recieved: " + recieved + " bytes");

        if (recieved <= 0)
            return;

        byte[] recData = new byte[recieved];
        Buffer.BlockCopy(_recieveBuffer, 0, recData, 0, recieved);

        string recvStr = Encoding.UTF8.GetString(recData, 0, recieved);
        Debug.Log("recvStr: " + recvStr);
        recieveData.Add(recvStr);



        _clientSocket.BeginReceive(_recieveBuffer, 0, _recieveBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), null);
    }
    /// 
    /// 關閉 Socket 連線.
    /// 
    public void Close()
    {
        _clientSocket.Shutdown(SocketShutdown.Both);
        _clientSocket.Close();
    }
}
