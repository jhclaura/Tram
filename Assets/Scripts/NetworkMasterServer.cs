using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;


public class NetworkMasterServer : NetworkBehaviour {

	public int MasterServerPort = 8888;

	// record all the players
	public Dictionary<string, MasterMsgTypes.Player> players = new Dictionary<string, MasterMsgTypes.Player>();

	public Text serverStatus;

	//---------- FUNCTINOS -----------

	public void InitializeServer()
	{
		if (NetworkServer.active) {
			Debug.LogError ("Already Initialized");
			return;
		}

		NetworkServer.Listen (MasterServerPort);

		// system msgs
		NetworkServer.RegisterHandler(MsgType.Connect, OnServerConnect);
		NetworkServer.RegisterHandler(MsgType.Disconnect, OnServerDisconnect);
		NetworkServer.RegisterHandler(MsgType.Error, OnServerError);

		// application msgs
		NetworkServer.RegisterHandler(MasterMsgTypes.RegisterClientId, OnServerRegisterClient);
		NetworkServer.RegisterHandler(MasterMsgTypes.UnregisterClientId, OnServerUnregisterClient);
		NetworkServer.RegisterHandler(MasterMsgTypes.RequestListOfClientsId, OnServerListClients);

		NetworkServer.RegisterHandler(MasterMsgTypes.ReportClientPositionId, OnReportClientPosition);

		DontDestroyOnLoad(gameObject);
	}

	public void ResetServer()
	{
		NetworkServer.Shutdown ();
	}

	bool CreatePlayerCheck(string typeName, string comment, string clientIp, int connectionID)
	{
		if (players.ContainsKey (typeName)) {
			return false;	// not sure if applicable in my case??? might delete it
		}

		MasterMsgTypes.Player newPlayer = new MasterMsgTypes.Player ();
		newPlayer.name = typeName;
		newPlayer.comment = comment;
		newPlayer.ip = clientIp;
		newPlayer.connectionId = connectionID;

		players [typeName] = newPlayer;
		Debug.Log ("Create new player " + typeName);

		Debug.Log ("Current:");
		foreach (var plys in players.Values) {
			Debug.Log ( "Player:" + plys.name);
		}

		return true;
	}

	// ---------- System Handlers ----------
	void OnServerConnect(NetworkMessage netMsg)
	{
		Debug.Log ("Server received client");
	}

	void OnServerDisconnect(NetworkMessage netMsg)
	{
		Debug.Log("Master lost client");

		// remove the associated client
		foreach (var player in players.Values)
		{
			if (player.connectionId == netMsg.conn.connectionId)
			{
				// tell other players?

				// remove room
				players.Remove(player.name);
				Debug.Log("Sense ["+player.name+"] closed because it left");
				break;
			}
		}
	}

	void OnServerError(NetworkMessage netMsg)
	{
		Debug.Log("ServerError from Master");
		//
		var errorMsg = netMsg.ReadMessage<ErrorMessage>();
		Debug.Log("Error:" + errorMsg.errorCode);
		//
	}

	// ---------- Application Handlers ----------
	void OnServerRegisterClient(NetworkMessage netMsg)
	{
		Debug.Log ("OnServerRegisterClient");
		var msg = netMsg.ReadMessage <MasterMsgTypes.RegisterClientMessage> ();
//		var sense = CreateSenseCheck (msg.gameTypeName);

		int result = (int)MasterMsgTypes.NetworkMasterServerEvent.RegistrationSucceeded;

		if (!CreatePlayerCheck (msg.playerName, msg.comment, netMsg.conn.address, netMsg.conn.connectionId)) {
			result = (int)MasterMsgTypes.NetworkMasterServerEvent.RegistrationFailedGameName;
		}
		var response = new MasterMsgTypes.RegisteredClientMessage ();
		response.resultCode = result;
		netMsg.conn.Send (MasterMsgTypes.RegisteredClientId, response);

		// not send to all players?

		// To-Do
		// send info to all players
		// send history info
		// build players locally with the info
	}

	void OnServerUnregisterClient(NetworkMessage netMsg)
	{
		Debug.Log("OnServerUnregisterClient");
		var msg = netMsg.ReadMessage<MasterMsgTypes.UnregisterClientMessage>();

		// find the player
		if (!players.ContainsKey (msg.playerName))
		{
			//error
			Debug.Log("OnServerUnregisterClient player not found: " + msg.playerName);
			return;
		}

		var ply = players[msg.playerName];
		if (ply.connectionId != netMsg.conn.connectionId)
		{
			//error
			Debug.Log("OnServerUnregisterHost connection mismatch:" + ply.connectionId);
			return;
		}
		players.Remove(msg.playerName);

		// to-do: tell other players

		var response = new MasterMsgTypes.RegisteredClientMessage();
		response.resultCode = (int)MasterMsgTypes.NetworkMasterServerEvent.UnregistrationSucceeded;
		netMsg.conn.Send(MasterMsgTypes.UnregisteredClientId, response);
	}

	void OnServerListClients(NetworkMessage netMsg)
	{
		//...
	}

	//
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

	//
	public void ServerBroadcastMsg(){
		var msg = new MasterMsgTypes.BroadcastTestMessage ();
//		msg.resultCode = 
		msg.contents = "server broadcast test!";
		NetworkServer.SendToAll (MasterMsgTypes.BroadcastTestId, msg);
	}

	public void ShowServerStatus(){
		string newStatus = "Server status-- channel num: "+ NetworkServer.numChannels +"\n";

		ReadOnlyCollection<NetworkConnection> connections = NetworkServer.connections;
//		Debug.Log (connections.Count);

		foreach( NetworkConnection c in connections ){
			// Why there's a null in here???
			if (c != null) {
				newStatus += "Connected client address: " + c.address
				+ ", connectionId: " + c.connectionId + "\n";
			}
		}
		serverStatus.text = newStatus;
	}

	// --------------------------------------------
//	void OnGUI()
//	{
//		if (NetworkServer.active)
//		{
//			GUI.Label(new Rect(400, 0, 200, 20), "Online port:" + MasterServerPort);
//			if (GUI.Button(new Rect(400, 20, 200, 20), "Reset Master Server"))
//			{
//				ResetServer();
//			}
//		}
//		else
//		{
//			if (GUI.Button(new Rect(400, 20, 200, 20), "Init Master Server"))
//			{
//				InitializeServer();
//			}
//		}

//		int y = 100;
//		foreach (var rooms in gameTypeRooms.Values)
//		{
//			GUI.Label(new Rect(400, y, 200, 20), "GameType:" + rooms.name);
//			y += 22;
//			foreach (var room in rooms.rooms.Values)
//			{
//				GUI.Label(new Rect(420, y, 200, 20), "Game:" + room.name + " addr:" + room.hostIp + ":" + room.hostPort);
//				y += 22;
//			}
//		}
//	}
}
