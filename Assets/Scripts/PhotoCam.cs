using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.IO;

// source: http://answers.unity3d.com/questions/773464/webcamtexture-correct-resolution-and-ratio.html#answer-1148424

public class PhotoCam : MonoBehaviour {

//	WebCamTexture webCamTexture;
	WebCamTexture frontCamTexture;
	WebCamTexture backCamTexture;
	WebCamTexture activeCamTexture;

	WebCamDevice frontCamDevice;
	WebCamDevice backCamDevice;
	WebCamDevice activeCamDevice;

	//
	public GameObject videoTarget;
	Renderer videoTargetRender;
	public GameObject cameraTarget;
	Renderer cameraTargetRender;

	// image rotation
	Vector3 rotationVector = new Vector3();
	// image uvRect
	Rect defaultRect = new Rect(0f,0f,1f,1f);
	Rect fixedRect = new Rect(0f,1f,1f,-1f);
	// image parent's scale
	Vector3 defaultScale = new Vector3(1f,1f,1f);
	Vector3 fixedScale = new Vector3(-1f,1f,1f);

	//
	public RawImage image;
	public RectTransform imageParent;
	public AspectRatioFitter imageFitter;

	//-------------------------------------------------------------------------
	void Start ()
	{
		if (WebCamTexture.devices.Length == 0)
		{
			Debug.Log ("No devices cameras found");
			return;
		}

//		frontCamers
		frontCamDevice = WebCamTexture.devices.Last ();
		backCamDevice = WebCamTexture.devices.First ();

		//
//		webCamTexture = new WebCamTexture ();
		frontCamTexture = new WebCamTexture(frontCamDevice.name);
		backCamTexture = new WebCamTexture (backCamDevice.name);

		// filter modes --> smoother
		frontCamTexture.filterMode = FilterMode.Trilinear;
		backCamTexture.filterMode = FilterMode.Trilinear;

		videoTargetRender = videoTarget.GetComponent<Renderer> ();
		cameraTargetRender = cameraTarget.GetComponent<Renderer> ();

//		videoTargetRender.material.mainTexture = webCamTexture;
//		webCamTexture.Play ();

		SetActiveCamera (backCamTexture);
	}

	public void SetActiveCamera(WebCamTexture cameraToUse)
	{
		if (activeCamTexture != null)
			activeCamTexture.Stop ();

		activeCamTexture = cameraToUse;
		activeCamDevice = WebCamTexture.devices.FirstOrDefault (device => device.name == cameraToUse.deviceName);

		image.texture = activeCamTexture;
		image.material.mainTexture = activeCamTexture;

		videoTargetRender.material.mainTexture = activeCamTexture;

		activeCamTexture.Play ();
	}

	public void SwitchCamera()
	{
		SetActiveCamera ( activeCamTexture.Equals(frontCamTexture) ? backCamTexture : frontCamTexture );
	}
	
	void Update () {
		if (activeCamTexture.width < 100) {
			Debug.Log ("Still waiting another frome for correct info");
			return;
		}

		// bunch of image corrections...

		// rotate image
		rotationVector.z = -activeCamTexture.videoRotationAngle;
		image.rectTransform.localEulerAngles = rotationVector;

		// set AspectRatioFitter's ratio
		float videoRatio = (float)activeCamTexture.width / (float)activeCamTexture.height;
		imageFitter.aspectRatio = videoRatio;
//
//		// unflip if vertically flipped
		image.uvRect = activeCamTexture.videoVerticallyMirrored ? fixedRect : defaultRect;
//
//		// mirror front-facing camera's image horizontally
		imageParent.localScale = activeCamDevice.isFrontFacing ? fixedScale : defaultScale;
	}

	public void TakePhoto() {
//		yield return new WaitForEndOfFrame ();
//
		Texture2D photo = new Texture2D (activeCamTexture.width, activeCamTexture.height);
		photo.SetPixels ( activeCamTexture.GetPixels() );
		photo.Apply ();
//
		cameraTargetRender.material.SetTexture ("_MainTex", photo);

		Debug.Log ("Took a photo and apply to texture.");
//
//		byte[] bytes = photo.EncodeToPNG ();
//		File.WriteAll
	}
}
