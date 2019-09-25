using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float X_POSITIVE_BOUND =  10.5f;
    private const float X_NEGATIVE_BOUND = -10.5f;
    private const float Z_POSITIVE_BOUND =  4.5f;
    private const float Z_NEGATIVE_BOUND = -4.5f;
    [SerializeField]
    private string playerId;
    // Start is called before the first frame update
    private void Start()
    {
        InputManager.Instance.playerMovement += movePlayer;
    }

    private void movePlayer(float horz,float vert,string id)
    { 
        if (id.Equals(this.playerId)&& (horz!=0 || vert!=0))
        {
            if (id.Equals("1"))
                Debug.Log("Simulation Time ");
            float x = setXInBounds(horz);
            float z = setZInBounds(vert);
            if (id.Equals(Communicator.PlayerId))
            {
                NetworkStream stream = Communicator.Client.GetStream();
                string data = horz.ToString() + ":" + vert.ToString() + ":" + this.playerId + "\n";
                byte[] buffer = System.Text.Encoding.ASCII.GetBytes(data);
                stream.Write(buffer, 0, buffer.Length);
            }
            gameObject.transform.position = new Vector3(x, gameObject.transform.position.y, z);
        }
    }
    private void OnDestroy()
    {
        InputManager.Instance.playerMovement -= movePlayer;
    }
    private float setXInBounds(float horz)
    {   
        float currentXCord = gameObject.transform.position.x;
        if (currentXCord + horz >= X_POSITIVE_BOUND)
            return X_POSITIVE_BOUND;
        if (currentXCord + horz <= X_NEGATIVE_BOUND)
            return X_NEGATIVE_BOUND;
        return currentXCord + horz; 
    }
    private float setZInBounds(float vert)
    {
        float currentZCord = gameObject.transform.position.z;
        if (currentZCord + vert >= Z_POSITIVE_BOUND)
            return Z_POSITIVE_BOUND;
        if (currentZCord + vert <= Z_NEGATIVE_BOUND)
            return Z_NEGATIVE_BOUND;
        return currentZCord + vert;
    }



}
