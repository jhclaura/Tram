using UnityEngine;
using System.Collections;

public class Glowing : MonoBehaviour {

	// Use this for initialization
	void Start () {
		LeanTween.alpha (gameObject.GetComponent<RectTransform>(), 0f, 1f).setLoopType (LeanTweenType.pingPong);
		LeanTween.scale (gameObject.GetComponent<RectTransform>(), new Vector3(.7f, .7f, .7f), 1f).setLoopType (LeanTweenType.pingPong);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
