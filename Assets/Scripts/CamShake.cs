using UnityEngine;
using System.Collections;

public class CamShake : MonoBehaviour {
	
	public float duration = 1.0f;
	public float speed = 1.0f;
	public float magnitude = 0.2f;
	
	public bool constant = true;
	private bool keepShake = false;

	private IEnumerator shakeCoroutine;

	//for lerping
	Vector3 tmpPos;
	float tParam = 0f;
	float valueToBeLerped = 0f;
	float lerpSpeed = 0.8f;


	// -------------------------------------------------------------------------
	public void StopShake() {
		StopCoroutine (shakeCoroutine);
	}

	// -------------------------------------------------------------------------
	public void RestartShake() {		
//		StopAllCoroutines();
//		StopCoroutine (shakeCoroutine);
		StartCoroutine (shakeCoroutine);
	}

	// -------------------------------------------------------------------------
	void Start() {
		shakeCoroutine = Shake ();
		StartCoroutine (shakeCoroutine);
	}

	// -------------------------------------------------------------------------
	void Update() {
		if (constant && keepShake) {
			keepShake = false;
			shakeCoroutine = Shake ();
			StartCoroutine (shakeCoroutine);
			Debug.Log("restart shake");
		}
	}
	
	// -------------------------------------------------------------------------
	IEnumerator Shake() {
		
		float elapsed = 0.0f;
		
		Vector3 originalCamPos = transform.localPosition;
		float randomStart = Random.Range(-1000.0f, 1000.0f);
		
		while (elapsed < duration) {
			
			elapsed += Time.deltaTime;			
			
			float percentComplete = elapsed / duration;			
			
			// We want to reduce the shake from full power to 0 starting half way through
			float damper = 1.0f - Mathf.Clamp(2.0f * percentComplete - 1.0f, 0.0f, 1.0f);
			
			// Calculate the noise parameter starting randomly and going as fast as speed allows
			float alpha = randomStart + speed * percentComplete;
			
			// map noise to [-1, 1]
			float x = Util.Noise.GetNoise(alpha, 0.0f, 0.0f) * 2.0f - 1.0f;
			float y = Util.Noise.GetNoise(0.0f, alpha, 0.0f) * 2.0f - 1.0f;
			
			x *= magnitude * damper;
			y *= magnitude * damper;
			
			transform.localPosition = new Vector3(x, y, originalCamPos.z);
				
			yield return null;
		}

		tmpPos = transform.localPosition;
		Debug.Log ("return");

		while (tParam < 1) {
			
			tParam += Time.deltaTime * lerpSpeed;
			valueToBeLerped = Mathf.Lerp(0, 1, tParam);
			
			transform.localPosition = Vector3.Lerp(tmpPos, originalCamPos, valueToBeLerped);
			
			yield return null;
		}


		transform.localPosition = originalCamPos;

//		StopCoroutine (shakeCoroutine);
		keepShake = true;
		tParam = 0f;
		valueToBeLerped = 0f;

		Debug.Log ("end of shake");
	}
}
