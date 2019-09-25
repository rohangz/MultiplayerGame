using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Text;
using UnityEngine.SceneManagement;
public class Communicator : MonoBehaviour
{
    public InputField inputField;
    private static TcpClient client;
    public static TcpClient Client 
    {
        get
        {
            return client;
        }

    }
    private static string playerId;
    public static string PlayerId 
    {
        get 
        {
            return playerId;
        }

    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void onButtonClick()
    {
       StartCoroutine(connectToGameServer());
    }
    IEnumerator connectToGameServer()
    {
       
        Debug.Log(inputField.text);
        int port = 3000;
        string ip = inputField.text;
        TcpClient cl = new TcpClient(ip,port);
        Communicator.client = cl;
        NetworkStream stream = cl.GetStream();
        byte[] buffer = new byte[cl.ReceiveBufferSize];
        int bytesRead = stream.Read(buffer, 0, cl.ReceiveBufferSize);
          string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
        playerId = dataReceived;
        Debug.Log(dataReceived);
        yield return null;

        // stream.Flush();
        //  bytesRead = stream.Read(buffer, 0, cl.ReceiveBufferSize);
          SceneManager.LoadScene(1);

    }

}
