using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Sockets;


public class movement : MonoBehaviour {
	public string sIP = "0.0.0.0";
	public const int sPort = 8023;
	public GameObject robot;
	private Socket _clientSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
  private byte[] _recieveBuffer = new byte[8142];
	Rigidbody rb;
	int VerticalSpeed;
	int HorizontalSpeed;

	private void ReceiveCallback(IAsyncResult AR)
    {
        //Check how much bytes are recieved and call EndRecieve to finalize handshake
        int recieved = _clientSocket.EndReceive(AR);

        if(recieved <= 0)
            return;

        //Copy the recieved data into new buffer , to avoid null bytes
        byte[] recData = new byte[recieved];
        Buffer.BlockCopy(_recieveBuffer,0,recData,0,recieved);

        //Process data here the way you want , all your bytes will be stored in recData
				string response = System.Text.Encoding.UTF8.GetString(recData);
				string[] data = response.Split(':');
				VerticalSpeed = Int32.Parse(data[0]) / 3;
				HorizontalSpeed = Int32.Parse(data[1]) / 3;
				Debug.Log(response);
				Debug.Log("Vertical speed: " + data[0] + "			Horizontal speed: " + data[1]);

        //Start receiving again
        _clientSocket.BeginReceive(_recieveBuffer,0,_recieveBuffer.Length,SocketFlags.None,new AsyncCallback(ReceiveCallback),null);
    }

	// Use this for initialization
	void Start () {
		try
    {
    	_clientSocket.Connect(new IPEndPoint(IPAddress.Parse(sIP),sPort));
    }
    catch(SocketException ex)
    {
    	Debug.Log(ex.Message);
    }
		Debug.Log("Connected");
    _clientSocket.BeginReceive(_recieveBuffer,0,_recieveBuffer.Length,SocketFlags.None,new AsyncCallback(ReceiveCallback),null);
	}

	void FixedUpdate () {
		rb = robot.GetComponent<Rigidbody>();
		rb.AddRelativeForce(Vector3.forward * VerticalSpeed);
		rb.AddRelativeForce(Vector3.right * HorizontalSpeed);

	}
}
