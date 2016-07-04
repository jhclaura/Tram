// by @torahhorse

using UnityEngine;
using System.Collections;

public class CameraFadeEffect_Lobby : MonoBehaviour
{
	public bool fadeInWhenSceneStarts = true;
	public Color fadeColor = Color.black;
	public float fadeTime = 5f;

	bool enterNextLevel = false;
	Vector3 pos;

	public bool toStartToFade;
	bool startFading;

	///
	///
	void Awake ()
	{
		if( fadeInWhenSceneStarts )
		{
			Fade();
		}

		pos = transform.position;
	}

	///
	///
	public void Fade()
	{
		CameraFade.StartAlphaFade(fadeColor, true, fadeTime);
	}

	///
	///
	void Update(){
		if (toStartToFade && !startFading) {
			startFading = true;
			CameraFade.StartAlphaFade(fadeColor, false, fadeTime, 0f);
		}

//		if(Time.time > 33f){
//			pos.z += 0.1f;
//			transform.position = pos;
//		}
//
//		if (Time.time > 43f && !enterNextLevel) {
//			enterNextLevel = true;
//			ChangeScene();
//		}

	}

	///
	///
	void ChangeScene(){
		Application.LoadLevel(1);
	}
}
