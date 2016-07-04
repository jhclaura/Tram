using UnityEngine;
using System.Collections;

public class Wiggle : MonoBehaviour {

	public float toRotation;
	public float toHeight;

	// Use this for initialization
	void Start () {
		LeanTween.rotateY (gameObject, toRotation, .7f).setLoopType (LeanTweenType.pingPong);
		LeanTween.moveY (gameObject, toHeight, .7f).setLoopType (LeanTweenType.pingPong);
	}
	
}
