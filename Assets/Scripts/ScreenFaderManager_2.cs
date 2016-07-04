using UnityEngine;
using System.Collections;

public class ScreenFaderManager_2 : MonoBehaviour {

	Animator anim;
	bool fadeInOver;

	// Use this for initialization
	void Awake () {
		anim = GetComponent<Animator>();
		fadeInOver = false;
	}

	void Start(){
		anim.SetTrigger("FadeIn");
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > 5f && !fadeInOver) {
			fadeInOver = true;
			//			CameraFade.StartAlphaFade(fadeColor, false, fadeTime, 0f, ChangeScene);
			transform.gameObject.SetActive(false);
		}
	}
}
