using UnityEngine;
using UnityEngine.Networking;

public class movement : MonoBehaviour {
	public string ipaddress = "0.0.0.0";
	public const int sPort = 8023;
	int sID;
	int ChanID;
	int connectID;

	public void SendSocketMessage() {
	  byte error;
	  byte[] buffer = new byte[1024];
	  Stream stream = new MemoryStream(buffer);
	  BinaryFormatter formatter = new BinaryFormatter();
	  formatter.Serialize(stream, "HelloServer");
	  int bufferSize = 1024;
	  NetworkTransport.Send(sID, connectID, ChanID, buffer, bufferSize, out error);
	}

	public void Connect() {
	  byte error;
	  connectID = NetworkTransport.Connect(sID, ipaddress, sPort, 0, out error);
	  Debug.Log("Connected to server. ConnectionId: " + connectID);
		SendSocketMessage();
	}

	// Use this for initialization
	void Start () {
		NetworkTransport.Init();
		ConnectionConfig config = new ConnectionConfig();
		ChanID = config.AddChannel(QosType.Reliable);
		int maxConnections = 10;
		HostTopology topology = new HostTopology(config, maxConnections);
		sID = NetworkTransport.AddHost(topology, sPort);
		Debug.Log("SocketId is: " + sID);
	}

	void FixedUpdate () {

	}
}
