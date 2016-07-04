using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class P_Generator : MonoBehaviour {

	public GameObject p_prefab;
	List<GameObject> passengers = new List<GameObject>();
	Vector3 leftDoorPosition = new Vector3(-43f, 0f, -90f);
	Vector3 RightDoorPosition = new Vector3(-43f, 0f, -48f);

	public bool carShouldStopForP = false;
	
	public GameObject streetCarToTake;
	streetcarMove_1 s_c_m_1;
	public bool generateP = false;

	int generatePosition = 0;

	void Start () {
//		GeneratePassenger ();

		carShouldStopForP = false;
		s_c_m_1 = streetCarToTake.GetComponent<streetcarMove_1> ();
	}
	
	void Update () {
		if(generateP){
			generateP=false;

			GeneratePassenger ();
		}
	}

	void GeneratePassenger(){
		int RorL = Random.Range (0, 2);
//		Debug.Log("pg: generate p: " + RorL);

		//left, (-41,0,-90)
		//right, (-41,0,-48)
		//v1
		if (RorL == 0) {
			//generate left
			GameObject p = (GameObject)Instantiate (p_prefab, leftDoorPosition, Quaternion.Euler(0,270,0)) as GameObject;
			P_ManMovement_2 p2 = p.GetComponent<P_ManMovement_2> ();
			p2.screetCarToTake = streetCarToTake;
			p2.getInFromLeftDoor = true;
			p.transform.parent = transform;
			passengers.Add(p);
			Debug.Log("pg: generate p: left + " + RorL);
		} else {
			//generate right
			GameObject p = (GameObject)Instantiate (p_prefab, RightDoorPosition, Quaternion.Euler(0,270,0)) as GameObject;
			P_ManMovement_2 p2 = p.GetComponent<P_ManMovement_2> ();
			p2.getInFromLeftDoor = false;
			p2.screetCarToTake = streetCarToTake;
			p.transform.parent = transform;
			passengers.Add(p);
			Debug.Log("pg: generate p: right + " + RorL);
		}

		//v2
//		if (generatePosition%2 == 1) {
//			//generate left
//			GameObject p = (GameObject)Instantiate (p_prefab, leftDoorPosition, Quaternion.Euler(0,270,0)) as GameObject;
//			P_ManMovement_2 p2 = p.GetComponent<P_ManMovement_2> ();
//			p2.screetCarToTake = streetCarToTake;
//			p2.getInFromLeftDoor = true;
//			p.transform.parent = transform;
//			passengers.Add(p);
//
//			generatePosition++;
//		} else {
//			//generate right
//			GameObject p = (GameObject)Instantiate (p_prefab, RightDoorPosition, Quaternion.Euler(0,270,0)) as GameObject;
//			P_ManMovement_2 p2 = p.GetComponent<P_ManMovement_2> ();
//			p2.getInFromLeftDoor = false;
//			p2.screetCarToTake = streetCarToTake;
//			p.transform.parent = transform;
//			passengers.Add(p);
//
//			generatePosition++;
//		}

	}
}
