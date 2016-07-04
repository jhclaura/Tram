// by @torahhorse

using UnityEngine;
using System.Collections;

public class CameraFadeOnStart_open : MonoBehaviour
{
	public bool fadeInWhenSceneStarts = true;
	public Color fadeColor = Color.black;
	public float fadeTime = 5f;

	bool enterNextLevel = false;
	Vector3 pos;

	bool StartToFade;

	void Awake ()
	{
		if( fadeInWhenSceneStarts )
		{
			Fade();
		}

		pos = transform.position;
	}
	
	public void Fade()
	{
		CameraFade.StartAlphaFade(fadeColor, true, fadeTime);
	}

	void Update(){
		if (Time.time > 38f && !StartToFade) {
			StartToFade = true;
			CameraFade.StartAlphaFade(fadeColor, false, fadeTime, 0f);
		}

		if(Time.time > 33f){
			pos.z += 0.1f;
			transform.position = pos;
		}

		if (Time.time > 43f && !enterNextLevel) {
			enterNextLevel = true;
			ChangeScene();
		}

//		Debug.Log ("Time.time: " + Time.time);
	}

	void ChangeScene(){
		Application.LoadLevel(1);
	}
}
