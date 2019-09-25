using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Client : MonoBehaviour
{
    private Socket clientSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
    private byte[] buffer = new byte[8142];


    private static Client gameClient;
    public static Client GameClient
    {
        get
        {
            return gameClient; 
        }

    }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        gameClient = this;
    }
    private void connectToGameServer()
    {
        try
        {
            clientSocket.Connect(new IPEndPoint(IPAddress.Loopback, 3000));
        }
        catch(Exception e)
        { 
        }
        clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(onReceiveServerMessage), null);

    }
    private void onReceiveServerMessage(IAsyncResult AR)
    {

    }

}
