using UnityEngine;
using System.Collections;

public class CamShake_v3 : MonoBehaviour {
	
	public float speed = .001f;
	Vector3 range;
	Vector3 originalCamPos;
	private Perlin noise = new Perlin();

	//for stop
	public streetcarMove_1 s_c_m;

	// -------------------------------------------------------------------------
	void Start() {
		originalCamPos = transform.localPosition;
		range = new Vector3 (.8f, .1f, .1f);

		s_c_m = transform.parent.gameObject.GetComponent<streetcarMove_1>();
	}

	// -------------------------------------------------------------------------
	void Update() {
		if (!s_c_m.carStop) {
			transform.localPosition = originalCamPos + Vector3.Scale (SmoothRandom.GetVector3 (speed), range);
		}
	}
	
}
