using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;
using UnityEngine.Networking;

public class TransportManager : NetworkBehaviour {

	int myReliableChannelId;
	int myUnreliableChannelId;

	string ip = "192.168.1.2";
	int socketId;
	int socketPort = 8888;

	int connectionId = -1;

	// Use this for initialization
	void Start () {

		//v.1 - appropriate for normal useage
		NetworkTransport.Init ();

		//v.2
//		GlobalConfig gConfig = new GlobalConfig ();
//		gConfig.MaxPacketSize = 500;
//		NetworkTransport.Init ( gConfig );

		// define several communication channels, with diff quality of services
		ConnectionConfig config = new ConnectionConfig ();
		myReliableChannelId = config.AddChannel (QosType.Reliable);		// will deliver message and assure that the message is delivered
		myUnreliableChannelId = config.AddChannel (QosType.Unreliable);	// will send message without any assurance, but will do this faster.

		int maxConnectinos = 10;
		// Topology definition - how many connections allowed and what connection config to use
		// Network Topology - the arrangement of the various elements (links, nodes, etc.) of a computer network
		HostTopology topology = new HostTopology (config, maxConnectinos);	//allow up to 10 connections

		// to open socket - listen to port 8888
		socketId = NetworkTransport.AddHost(topology, socketPort);
		Debug.Log ("Socket opened, socketId: " + socketId);
	}

	// connect through the socket
	public void Connect() {
		if (connectionId != -1){
			Debug.Log("Already connected!");
			return;
		}

		byte error;
		// send connect request to peer with ip “localhost” and port 8888
		connectionId = NetworkTransport.Connect (socketId, ip, socketPort, 0, out error);
		Debug.Log ("Connect to server. ConnectionId: " + connectionId);
	}

	public void SendSocketMessage(){
		byte error;
		byte[] buffer = new byte[1024];
		Stream stream = new MemoryStream (buffer);
		BinaryFormatter formatter = new BinaryFormatter ();
		formatter.Serialize (stream, "Hello Server");

		int bufferSize = 1024;
		NetworkTransport.Send (socketId, connectionId, myReliableChannelId, buffer, bufferSize, out error);
	}

	void Update(){

		int recHostId;
		int recConnectionId;
		int recChannelId;
		byte[] recBuffer = new byte[1024];	// restore the coming data!
		int bufferSize = 1024;
		int recDataSize;
		byte error;

		NetworkEventType recEvent = NetworkTransport.Receive (out recHostId, out recConnectionId, out recChannelId, recBuffer, bufferSize, out recDataSize, out error); 

		switch (recEvent)
		{
			case NetworkEventType.Nothing:
				break;
			case NetworkEventType.ConnectEvent:
//				Debug.Log ("incoming connection event received!");
				Debug.Log(string.Format("incoming connection event received with connectionId: {0}, recHostId: {1}, recChannelId: {2}", recConnectionId, recHostId, recChannelId));
				break;
			case NetworkEventType.DataEvent:
				Stream stream = new MemoryStream (recBuffer);	// !!! *o* !!!
				BinaryFormatter formatter = new BinaryFormatter ();
				string message = formatter.Deserialize (stream) as string;
				Debug.Log(string.Format("incoming message with connectionId: {0}, recHostId: {1}, recChannelId: {2}", recConnectionId, recHostId, recChannelId));
				Debug.Log ("msg: " + message);
				break;
			case NetworkEventType.DisconnectEvent:
				if (recConnectionId == connectionId) {
					Debug.Log ("I disconnected");
					connectionId = -1;
				}
				else
					Debug.Log ("remote client event disconnected");
				break;
		}
	}
}
