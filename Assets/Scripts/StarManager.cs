using UnityEngine;
using System.Collections;

public class StarManager : MonoBehaviour {

	Vector3 rot;

	// Use this for initialization
	void Start () {
		rot = transform.eulerAngles;
	}
	
	// Update is called once per frame
	void Update () {
//		rot.y += 0.1f;
//		transform.eulerAngles = rot;
	}
}
