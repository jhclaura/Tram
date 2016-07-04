using UnityEngine;
using System.Collections;

public class TramCreator : MonoBehaviour {

	public GameObject tramPrefab;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < 5; i++) {
			Instantiate(tramPrefab, new Vector3( Random.Range(-20,20), 0, -100), Quaternion.identity);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
