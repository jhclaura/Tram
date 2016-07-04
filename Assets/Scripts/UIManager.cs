using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

	public Button resetButton;
	public Button startButton;

	public NetworkManager netManager;
//	public TramServer serverS;
//	public TramClient clientS;

	// record all the players
	public Dictionary<string, MasterMsgTypes.Player> players = new Dictionary<string, MasterMsgTypes.Player>();

	public Text networkingStatus;

	NetworkClient theClient = null;
	string playerName = "";

	//----------------------------------------------
	void Awake()
	{
		netManager = GetComponent<NetworkManager> ();
	}
	
	//----------------------------------------------
	void Update () {
		if (NetworkServer.active && !resetButton.interactable) {
			resetButton.interactable = true;
			startButton.interactable = false;
		} else if (!NetworkServer.active && resetButton.interactable) {
			resetButton.interactable = false;
			startButton.interactable = true;
		}
	}

	//------------- Manager Functions --------------
	public void M_StartServer()
	{
		netManager.StartServer ();

		SetupServer ();

		Debug.Log ( netManager.startPositions.Count );
	}

	public void M_StartClient( InputField inputName )
	{
		networkingStatus.text = "Client Status";

		if (inputName.text == "") {
			networkingStatus.text = "Please input your name.";
			return;
		} else {
			playerName = inputName.text;
		}

		netManager.StartClient ();
		theClient = netManager.client;

		SetupClient ();
	}

	public void M_ChangeScene(string sceneName)
	{
		netManager.ServerChangeScene (sceneName);
	}

	//
	void SetupServer()
	{
		NetworkServer.RegisterHandler(MasterMsgTypes.ReportClientPositionId, OnReportClientPosition);
	}

	void SetupClient ()
	{
		theClient.RegisterHandler (MasterMsgTypes.BroadcastTestId, OnBroadcastTest);

		// register client with Name
		var msg = new MasterMsgTypes.RegisterClientMessage();
		msg.playerName = playerName;
		theClient.Send (MasterMsgTypes.RegisterClientId, msg);
	}

	/////////////////////////////////////////////////////////////////////
	///							Server stuff						  ///
	/////////////////////////////////////////////////////////////////////

	// ------------------------------------------------------------------
	// Server's events handler
	// ------------------------------------------------------------------ 
	void OnReportClientPosition(NetworkMessage netMsg)
	{
		Debug.Log("OnReportClientPosition");
		var msg = netMsg.ReadMessage<MasterMsgTypes.ReportClientPositionMessage>();

		// find the player
		if (!players.ContainsKey (msg.playerName))
		{
			//error
			Debug.Log("OnReportClientPosition player not found: " + msg.playerName);
			return;
		}

		var ply = players[msg.playerName];
		if (ply.connectionId != netMsg.conn.connectionId)
		{
			//error
			Debug.Log("OnReportClientPosition connection mismatch, ply.connectionId: " + ply.connectionId
				+ ", netMsg.conn.connectionId: " + netMsg.conn.connectionId);
			return;
		}

		Debug.Log ("S: Got report from player: " + msg.playerName + " about its position: " + msg.playerPosition);
	}

	// ------------------------------------------------------------------
	// Server's functions
	// ------------------------------------------------------------------
	public void ShowServerStatus(){
		ReadOnlyCollection<NetworkConnection> connections = NetworkServer.connections;

		string newStatus = "Server status-- channel num: "+ NetworkServer.numChannels 
			+", connection num: " + connections.Count + "\n";

		foreach( NetworkConnection c in connections ){
			// Why there's a null in here???
			if (c != null) {
				newStatus += "Connected client address: " + c.address
					+ ", connectionId: " + c.connectionId + "\n";
			}
		}
		networkingStatus.text = newStatus;
	}

	//
	public void ServerBroadcastMsg(){
		var msg = new MasterMsgTypes.BroadcastTestMessage ();
		//		msg.resultCode = 
		msg.contents = "server broadcast test!";
		NetworkServer.SendToAll (MasterMsgTypes.BroadcastTestId, msg);

		networkingStatus.text = msg.contents;
	}

	/////////////////////////////////////////////////////////////////////
	///							Client stuff						  ///
	/////////////////////////////////////////////////////////////////////

	// ------------------------------------------------------------------
	// Client's events handler
	// ------------------------------------------------------------------
	void OnBroadcastTest(NetworkMessage netMsg)
	{
		var msg = netMsg.ReadMessage<MasterMsgTypes.BroadcastTestMessage> ();
		Debug.Log ("C: Got server's broadcast msg: " + msg.contents);
		networkingStatus.text = "C: Got server's broadcast msg: " + msg.contents;
	}

	// ------------------------------------------------------------------
	// Client's functions
	// ------------------------------------------------------------------
	void ReportClientPosition()
	{
		var msg = new MasterMsgTypes.ReportClientPositionMessage ();
		msg.playerName = playerName;
		msg.playerPosition = new Vector3 ();
		theClient.SendUnreliable (MasterMsgTypes.ReportClientPositionId, msg);
	}
}
