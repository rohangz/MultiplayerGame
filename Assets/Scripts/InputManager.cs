using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static readonly string HORIZONTAL_AXIS = "Horizontal";
    public static readonly string VERTICAL_AXIS = "Vertical";

    public event System.Action<float, float, string> playerMovement;
    private string start = "";
    private Coroutine managePlayerInputsRoutine;
    private Coroutine manageOpponentInputRoutine;
    // Start is called before the first frame update
    private NetworkStream stream;
    private static InputManager instance;
    public static InputManager Instance
    {
        get
        {
            return instance;
        }
    }
    public static MovementInputs movement;


    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        movement = new MovementInputs();
         managePlayerInputsRoutine = StartCoroutine(playerMovementRoutine());
         manageOpponentInputRoutine = StartCoroutine(opponentMovementRoutine());
   //    readInputFromServerTask();

    }

    IEnumerator playerMovementRoutine()
    {
        while(true)
        {
         
            float horz = Input.GetAxis(HORIZONTAL_AXIS);
            float vert = Input.GetAxis(VERTICAL_AXIS);
            if (playerMovement != null)
                playerMovement(horz, vert, Communicator.PlayerId);
            yield return null;
        }

    }
    IEnumerator opponentMovementRoutine()
    {
        while (true)
        {

            readInputFromServerTask();
            if (playerMovement != null)
                playerMovement(movement.horz, movement.vert, movement.playerId);
            movement = new MovementInputs();
            yield return null;
        }

    }    
    private MovementInputs getOpponentInput()
    {
        var stream = Communicator.Client.GetStream();
        if (stream == null)
            return null;
        else
        {
            byte[] buffer = new byte[Communicator.Client.ReceiveBufferSize];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
           // Debug.Log(Encoding.ASCII.GetString(buffer,0,bytesRead));
            MovementInputs inputs = new MovementInputs(Encoding.ASCII.GetString(buffer));
            Debug.LogError("Player Id "+inputs.playerId.Length.ToString());
            if (playerMovement != null)
            {
                string newId = (Communicator.PlayerId == "1") ?"0":"1";
                //   playerMovement(inputs.horz, inputs.vert, newId);
                inputs.playerId = newId;
                movement = inputs;

            }
            return inputs;
        }

    }


    private async void readInputFromServerTask()
    {
         Task<MovementInputs> task = new Task<MovementInputs>(getOpponentInput);
            task.Start();
        MovementInputs inputs = await task;
       
    }




}
public class MovementInputs
{
    public float horz;
    public float vert;
    public string playerId;

    public MovementInputs(string data)
    {
        string[] datas = data.Split(':');
        if (datas.Length != 3)
            throw new Exception("Error Rohan Exception");
        horz = float.Parse(datas[0]);
        vert = float.Parse(datas[1]);
        playerId = datas[2];
    }
    public MovementInputs()
    {
        horz = 0;
        vert = 0;
        playerId = "";
     }

}
