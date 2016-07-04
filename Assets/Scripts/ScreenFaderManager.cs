using UnityEngine;
using System.Collections;

public class ScreenFaderManager : MonoBehaviour {

	Animator anim;
	bool StartToFade;

	// Use this for initialization
	void Awake () {
		anim = GetComponent<Animator>();
		StartToFade = false;
	}

	void Start(){
		anim.SetTrigger("FadeIn");
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > 38f && !StartToFade) {
			StartToFade = true;
//			CameraFade.StartAlphaFade(fadeColor, false, fadeTime, 0f, ChangeScene);
			anim.SetTrigger("FadeOut");
		}
	}
}
