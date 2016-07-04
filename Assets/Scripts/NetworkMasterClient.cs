using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class NetworkMasterClient : MonoBehaviour {

	public string MasterServerIpAddress;
	public int MasterServerPort = 8888;
	public string playerName = "Eye";
	public string comment = "a eye";

	[SerializeField]
	public int yoffset = 0;

	string PlayerName = "";

//	MasterMsgTypes.Sense sense = null;

	public NetworkClient client = null;

//	static NetworkMasterClient singleton;

	public InputField inputPlayerName;

//	void Awake()
//	{
//		if (singleton == null) {
//			singleton = this;
//			DontDestroyOnLoad (gameObject);
//		} else {
//			Destroy (gameObject);
//		}
//	}

	public void InitializeClient()
	{
		if (client != null) {
			Debug.LogError ("C: Already connected");
			return;
		}

		client = new NetworkClient ();
		client.Connect (MasterServerIpAddress, MasterServerPort);

		// system msgs
		client.RegisterHandler(MsgType.Connect, onClientConnect);
		client.RegisterHandler(MsgType.Disconnect, onClientError);
		client.RegisterHandler(MsgType.Error, onClientError);

		// application msgs
		client.RegisterHandler(MasterMsgTypes.RegisteredClientId, OnRegisteredClient);
		client.RegisterHandler(MasterMsgTypes.UnregisteredClientId, OnUnregisteredClient);
		client.RegisterHandler(MasterMsgTypes.ListOfClientsId, OnListOfClients);
		client.RegisterHandler(MasterMsgTypes.BroadcastTestId, OnBroadcastTest);

		DontDestroyOnLoad (gameObject);
	}

	public void ResetClient()
	{
		if (client == null)
			return;

		client.Disconnect ();
		client = null;
	}

	public bool isConnected
	{
		get
		{
			if (client == null)
				return false;
			else
				return client.isConnected;
		}
	}

	// --------- System Handlers ----------
	void onClientConnect(NetworkMessage netMsg)
	{
		Debug.Log ("C: Client connected to Master");
	}

	void onClientDisconnect(NetworkMessage netMsg)
	{
		Debug.Log ("C: Client disconnected from Master");
		ResetClient ();
		OnFailedToConnectToMasterServer ();
	}

	void onClientError(NetworkMessage netMsg)
	{
		Debug.Log ("C: ClientError from Master");
		//
		var errorMsg = netMsg.ReadMessage<ErrorMessage>();
		Debug.Log("Error:" + errorMsg.errorCode);
		//
		OnFailedToConnectToMasterServer ();
	}

	// ---------- Application Handlers ----------

	void OnRegisteredClient(NetworkMessage netMsg)
	{
		var msg = netMsg.ReadMessage<MasterMsgTypes.RegisteredClientMessage>();
		OnServerEvent((MasterMsgTypes.NetworkMasterServerEvent)msg.resultCode);
	}

	void OnUnregisteredClient(NetworkMessage netMsg)
	{
		var msg = netMsg.ReadMessage<MasterMsgTypes.RegisteredClientMessage>();
		OnServerEvent((MasterMsgTypes.NetworkMasterServerEvent)msg.resultCode);
	}

	void OnListOfClients(NetworkMessage netMsg)
	{
//		var msg = netMsg.ReadMessage<MasterMsgTypes.ListOfClientsMessage>();
//		hosts = msg.hosts;
//		OnServerEvent(MasterMsgTypes.NetworkMasterServerEvent.HostListReceived);
	}

	void OnBroadcastTest(NetworkMessage netMsg)
	{
		var msg = netMsg.ReadMessage<MasterMsgTypes.BroadcastTestMessage>();
		Debug.Log ("C: Got server's broadcast msg: " + msg.contents);
	}

	// --------------------------------------------

	public void RegisterClient()
	{
		if (!isConnected) {
			Debug.LogError ("RegisterHost not connected");
			return;
		}

		var msg = new MasterMsgTypes.RegisterClientMessage ();

		if (inputPlayerName.text != "")
			playerName = inputPlayerName.text;
		
		msg.playerName = playerName;
		msg.comment = comment;
		client.Send (MasterMsgTypes.RegisterClientId, msg);

		PlayerName = playerName;
	}

	public void RequestClientList()
	{
		if (!isConnected)
		{
			Debug.LogError("RequestHostList not connected");
			return;
		}

		var msg = new MasterMsgTypes.ReguestClientListMessage();
		msg.playerName = PlayerName;
		client.Send(MasterMsgTypes.RequestListOfClientsId, msg);
	}

	public void UnregisterClient()
	{
		if (!isConnected)
		{
			Debug.LogError("UnregisterHost not connected");
			return;
		}

		var msg = new MasterMsgTypes.UnregisterClientMessage();
		msg.playerName = PlayerName;
		client.Send(MasterMsgTypes.UnregisterClientId, msg);
		PlayerName = "";

		Debug.Log("send UnregisterHost");
	}

	public virtual void OnFailedToConnectToMasterServer()
	{
		Debug.Log("OnFailedToConnectToMasterServer");
	}

	public virtual void OnServerEvent(MasterMsgTypes.NetworkMasterServerEvent evt)
	{
		Debug.Log("C: OnServerEvent " + evt);
	}

	//
	public void ReportClientPosition()
	{
		if (!isConnected) {
			Debug.LogError ("ReportClientPosition not connected");
			return;
		}

		var msg = new MasterMsgTypes.ReportClientPositionMessage ();
		msg.playerName = playerName;
		msg.playerPosition = new Vector3();
//		client.Send (MasterMsgTypes.ReportClientPositionId, msg);
		// send through unreliable channel to speed up
		client.SendUnreliable (MasterMsgTypes.ReportClientPositionId, msg);
	}
}
