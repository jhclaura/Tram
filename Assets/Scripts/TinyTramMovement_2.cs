using UnityEngine;
using System.Collections;

public class TinyTramMovement_2 : MonoBehaviour {

//	public GameObject dummyHead;
	public Color[] colors;
	Vector3 center;
	Vector3 oriPos;

	// start: x: 30~-30, z:-90
	// color: turquoise ~ pink
	void Start() {
//		dummyHead = GameObject.FindGameObjectWithTag ("dummyHead");
		center = new Vector3();
		oriPos = gameObject.transform.position;

		LeanTween.moveZ (transform.gameObject, 100f, 10f).setDelay( Random.Range(1f,10f) ).setLoopClamp().setOnComplete( ResetTram ).setOnCompleteOnRepeat(true);
		gameObject.GetComponent<SpriteRenderer> ().color = colors[ Random.Range(0,colors.Length) ];
	}
	
	void Update () {
		transform.LookAt(center, -Vector3.up);
	}

	void ResetTram() {
		Vector3 newPos = new Vector3 ( Random.Range(-15,15), 0, -100);
		gameObject.transform.position = newPos;
		gameObject.GetComponent<SpriteRenderer> ().color = colors[ Random.Range(0,colors.Length) ];
	}
}
