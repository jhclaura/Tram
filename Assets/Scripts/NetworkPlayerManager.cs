using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkPlayerManager : NetworkBehaviour {

	public GameObject head;
	public GameObject stereoRender;
	public GameObject camShakerHolder;
	public CamShaker camShaker;
	public FirstPersonDrifter_2 fpd;
	public DummyPersonDrifter dpd;
	public DummyCamAttacher dca;

	public GameObject paperHead;
	public GameObject paperHeadFace;
	Renderer faceRenderer;

	Renderer[] renderers;
	GvrHead_vL gvrhead;
	Transform targetRot;

	Vector3 spawnedPosition;

	void Awake()
	{
		spawnedPosition = gameObject.transform.position;
	}

	// Use this for initialization
	void Start () {
		renderers = GetComponentsInChildren<Renderer> ();
		faceRenderer = paperHeadFace.GetComponent<Renderer> ();
//		Debug.Log ("Player id: " + this.playerControllerId);
	}

	public override void OnStartServer(){
		Debug.Log ("OnStartServer");
		dca.spawnedPos = spawnedPosition;
	}

	public override void OnStartClient(){
//		Debug.Log ("OnStartClient");
		dca.spawnedPos = spawnedPosition;
		//
		faceRenderer.material = new Material(Shader.Find("Mobile/Unlit"));
	}
	
	public override void OnStartLocalPlayer(){
//		Debug.Log ("OnStartLocalPlayer");
//		gvrViewer.enabled = true;

		// dynamically create GvrViewer, cuz doesn't allow to exist in the begining
		GvrViewer.Create ();

		gvrhead = head.GetComponent<GvrHead_vL> ();
//		gvrhead.targetRot = GameObject.Find("StreetCar").transform;
		gvrhead.targetRot = GameObject.FindWithTag("StreetCar").transform;

		head.SetActive (true);

		stereoRender.SetActive (true);
		dca.enabled = false;
		//
		camShaker.spawnedPos = spawnedPosition;
		//
		camShaker.enabled = true;
		dpd.enabled = false;
		fpd.enabled = true;
		//
		paperHead.SetActive(false);

		gameObject.name = "LOCAL_PLAYER";
		base.OnStartLocalPlayer ();
	}

	public override void OnNetworkDestroy()
	{
		// to destroy the CamShaker, which is changed to be a child of StreetCar
		Destroy( camShakerHolder );
	}
}
