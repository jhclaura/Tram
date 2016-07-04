using UnityEngine;
using System.Collections;

public class DummyCamAttacher : MonoBehaviour {

	public GameObject streetCar;
	Vector3 dummyPosition;
	public Vector3 spawnedPos;

	// -------------------------------------------------------------------------
	void Awake() {
		streetCar = GameObject.Find("StreetCar");

		dummyPosition = new Vector3 (0, 0, -3);
	}

	// -------------------------------------------------------------------------
	void Start() {
		transform.SetParent( streetCar.transform, false );
		transform.localPosition = spawnedPos;
	}
}
