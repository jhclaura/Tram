using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class MasterMsgTypes
{
	public enum NetworkMasterServerEvent
	{
		RegistrationFailedGameName,
		RegistrationFailedGameType,
		RegistrationFailedNoServer,
		RegistrationSucceeded,
		UnregistrationSucceeded,
		HostListReceived,
	}

	// ---------- client to masterserver Ids ----------
	public const short RegisterClientId = 150;
	public const short UnregisterClientId = 151;
	public const short RequestListOfClientsId = 152;

	public const short ReportClientPositionId = 153;

	// ---------- masterserver to client Ids ----------
	public const short RegisteredClientId = 160;
	public const short UnregisteredClientId = 161;
	public const short ListOfClientsId = 162;

	public const short BroadcastTestId = 163;


	// ---------- client to masterserver msgs ----------
	public class RegisterClientMessage: MessageBase
	{
		public string playerName;
		public string comment;
		public int hostPort;
	}

	public class UnregisterClientMessage: MessageBase
	{
		public string playerName;
	}

	public class ReguestClientListMessage: MessageBase
	{
		public string playerName;
	}

	public class ReportClientPositionMessage: MessageBase
	{
		public string playerName;
		public Vector3 playerPosition;
	}

	// ---------- masterserver to client msgs ----------
	public struct Player
	{
		public string name;
		public string comment;
		public string ip;
		public int port;
		public int connectionId;
	}

	public class ListOfClientsMessage: MessageBase
	{
		public int resultCode;
		public Player[] players;
	}

	public class RegisteredClientMessage: MessageBase
	{
		public int resultCode;
	}

	public class BroadcastTestMessage: MessageBase
	{
		public string contents;
	}
}
