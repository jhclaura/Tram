using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TramNetworkManager : NetworkManager {

	[SerializeField] private GameObject sceneCamera;
	[SerializeField] GameObject m_PlayerPrefab_Intro;
	[SerializeField] GameObject m_PlayerPrefab_Tram;

	public GameObject playerPrefab_intro      { get { return m_PlayerPrefab_Intro; }  set { m_PlayerPrefab_Intro = value; } }
	public GameObject playerPrefab_tram     { get { return m_PlayerPrefab_Tram; }  set { m_PlayerPrefab_Tram = value; } }
	GameObject sceneCamera2;

	//----------------------------------------------
	public override void OnServerConnect(NetworkConnection conn)
	{
		Debug.Log ("Manager: a new client connects");
	}

	//----------------------------------------------
	public override void OnStartClient(NetworkClient client)
	{	
		Debug.Log ("Manager: OnStartClient, hide scene camera");
		HideSceneCamera ();
		ClientScene.RegisterPrefab (m_PlayerPrefab_Intro);
		ClientScene.RegisterPrefab (m_PlayerPrefab_Tram);
	}

	public override void OnStopClient()
	{
		ShowSceneCamera ();
	}

	//----------------------------------------------
	public override void OnClientSceneChanged(NetworkConnection conn)
	{
		Debug.Log ("Manager: OnClientSceneChanged, hide scene camera");
		sceneCamera2 = GameObject.Find("DefaultCamera");
		sceneCamera2.SetActive (false);

		base.OnClientSceneChanged (conn);
	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{

//		Debug.Log ( NetworkManager.networkSceneName );

		GameObject playerPrefabToBe;

		if (NetworkManager.networkSceneName == "") {
			// first(intro) scene
			playerPrefabToBe = playerPrefab_intro;
		} else {
			// second(tram) scene
			playerPrefabToBe = playerPrefab_tram;
		}

		if (playerPrefabToBe == null)
		{
			if (LogFilter.logError) { Debug.LogError("The PlayerPrefab is empty on the NetworkManager. Please setup a PlayerPrefab object."); }
			return;
		}

		if (playerPrefabToBe.GetComponent<NetworkIdentity>() == null)
		{
			if (LogFilter.logError) { Debug.LogError("The PlayerPrefab does not have a NetworkIdentity. Please add a NetworkIdentity to the player prefab."); }
			return;
		}

		if (playerControllerId < conn.playerControllers.Count  && conn.playerControllers[playerControllerId].IsValid && conn.playerControllers[playerControllerId].gameObject != null)
		{
			if (LogFilter.logError) { Debug.LogError("There is already a player at that playerControllerId for this connections."); }
			return;
		}

		GameObject player;
		Transform startPos = GetStartPosition();
		if (startPos != null)
		{
			player = (GameObject)Instantiate(playerPrefabToBe, startPos.position, startPos.rotation);
		}
		else
		{
			player = (GameObject)Instantiate(playerPrefabToBe, Vector3.zero, Quaternion.identity);
		}

		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
	}

	//----------------------------------------------
	private void HideSceneCamera()
	{
		if (sceneCamera)
			sceneCamera.SetActive (false);
	}

	private void ShowSceneCamera()
	{       
		if(sceneCamera)
			sceneCamera.SetActive(true);
	}
}
