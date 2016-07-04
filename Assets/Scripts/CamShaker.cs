using UnityEngine;
using System.Collections;

public class CamShaker : MonoBehaviour {
	
	public float speed = 1f;
	Vector3 range;
	public Vector3 originalCamPos;
	private Perlin noise = new Perlin();

	//for stop
	public streetcarMove_1 s_c_m;
	public GameObject streetCar;
	public Vector3 spawnedPos;

	// -------------------------------------------------------------------------
	void Awake()
	{
		streetCar = GameObject.Find("StreetCar");
	}

	// -------------------------------------------------------------------------
	void Start()
	{
		range = new Vector3 (.8f, .1f, .1f);

		transform.SetParent( streetCar.transform, false );
//		originalCamPos = transform.localPosition;
		originalCamPos = transform.localPosition + spawnedPos;
		Debug.Log ("originalCamPos: " + originalCamPos);

		s_c_m = transform.parent.gameObject.GetComponent<streetcarMove_1>();
	}

	// -------------------------------------------------------------------------
	void Update() {
		if (!s_c_m.carStop) {
			transform.localPosition = originalCamPos + Vector3.Scale (SmoothRandom.GetVector3 (speed), range);
		}
	}
	
}
