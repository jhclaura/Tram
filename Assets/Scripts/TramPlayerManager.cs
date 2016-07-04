using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TramPlayerManager : NetworkBehaviour {

	public GameObject head;
	public GameObject stereoRender;
	public GameObject camShakerHolder;
	public CamShaker camShaker;
	public FirstPersonDrifter_2 fpd;
	public DummyPersonDrifter dpd;
	public DummyCamAttacher dca;

	public int sceneNum = 0;
	public GvrHead_vL headScript;

	Renderer[] renderers;
	GvrHead_vL gvrhead;
	Transform targetRot;

	GvrViewer gvrViewer;

	void Awake () {
		renderers = GetComponentsInChildren<Renderer> ();
//		Debug.Log ("Player be created, id: " + this.playerControllerId);

		if (sceneNum == 0) {
			// if in Scene_Intro
			// no real body
			camShakerHolder.SetActive (false);
		} else {
			// in Scene_Tram
			// has ready body and sits in Tram
			camShakerHolder.SetActive (true);
			headScript.trackPosition = true;
			headScript.trackRotation = true;
		}
	}

//	public override void OnStartServer()
//	{
//		GvrViewer.Create ();
//		gvrViewer = GetComponent<GvrViewer> ();
//		gvrViewer.VRModeEnabled = false;
//
////		dca.enabled = false;
////		camShaker.enabled = true;
////		dpd.enabled = false;
////		fpd.enabled = true;
//
//		head.SetActive (true);
//		stereoRender.SetActive (true);
//
//		gameObject.name = "SERVER_PLAYER";
//		base.OnStartServer ();
//	}

	public override void OnStartLocalPlayer()
	{

		// dynamically create GvrViewer, cuz doesn't allow to exist in the begining
		GvrViewer.Create ();
		gvrViewer = UnityEngine.Object.FindObjectOfType<GvrViewer> ();

		if (sceneNum == 0) {
			gvrViewer.VRModeEnabled = false;
		} else {
			dca.enabled = false;
			camShaker.enabled = true;
			dpd.enabled = false;
			fpd.enabled = true;

			headScript.targetRot = GameObject.Find("StreetCar").transform;
		}
			
		head.SetActive (true);
		stereoRender.SetActive (true);

		gameObject.name = "LOCAL_PLAYER";
		base.OnStartLocalPlayer ();
	}

	public override void OnNetworkDestroy()
	{
		// to destroy the CamShaker, which is changed to be a child of StreetCar
		Destroy( camShakerHolder );
	}
}
