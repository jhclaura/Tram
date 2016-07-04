using UnityEngine;
using System.Collections;

public class P_ManMovement_2 : MonoBehaviour {
	
	Animator anim;
	int walkinHash = Animator.StringToHash("p_walkin");
	int lookaroundHash = Animator.StringToHash("p_lookaround");
	int turnleftHash = Animator.StringToHash("p_turnleft");
	int turnrightHash = Animator.StringToHash("p_turnright");
	
	bool insideCar;
	public GameObject screetCarToTake;
	
	//walk
	Vector3 position = new Vector3(0,0,0);
	public bool getInFromLeftDoor = false;
	bool firstTurn = false;
	
	AnimatorClipInfo[] aniInfos;
	//stop
	P_Generator pg;
	
	// Use this for initialization
	void Start () {
		insideCar = false;

		anim = GetComponent<Animator> ();
		anim.SetTrigger ("WalkIn");
		//		Debug.Log ( standHash );
		//		Debug.Log ( anim.GetCurrentAnimatorStateInfo(0).shortNameHash );
		
		position = transform.position;

		// turn setting
		if (getInFromLeftDoor) {
			anim.SetBool ("TurnRight", true);
			anim.SetBool ("TurnLeft", false);
		} else {
			anim.SetBool ("TurnRight", false);
			anim.SetBool ("TurnLeft", true);
		}

		// walk
		LeanTween.moveLocalZ (transform.gameObject, 21f, 2.5f).setEase (LeanTweenType.linear).setDelay(1f);

		//
		pg = transform.parent.gameObject.GetComponent<P_Generator>();
		pg.carShouldStopForP = true;


	}
	
	// Update is called once per frame
	void Update () {
		
//		if(!insideCar){
//			position.x -= 0.1f;
//			transform.position=position;
//		}

		// debug
//		if(anim.GetNextAnimatorStateInfo(0).shortNameHash == lookaroundHash){
//			Debug.Log("anim.GetNextAnimatorStateInfo --> lookaround");
//		}
//		if(anim.IsInTransition (0)){
//			Debug.Log("anim.IsInTransition (0)");
//		}
//		Debug.Log (anim.GetNextAnimatorStateInfo(0).shortNameHash);
		if(anim.GetNextAnimatorStateInfo(0).shortNameHash == turnleftHash){
			Debug.Log("anim.GetNextAnimatorStateInfo --> turnleftHash");
		}
		if(anim.GetNextAnimatorStateInfo(0).shortNameHash == turnrightHash){
			Debug.Log("anim.GetNextAnimatorStateInfo --> turnrightHash");
		}
		
		// walk in
//		if(!insideCar && anim.IsInTransition(0) && anim.GetNextAnimatorStateInfo(0).shortNameHash == lookaroundHash){
		if(!insideCar && anim.GetNextAnimatorStateInfo(0).shortNameHash == lookaroundHash){
			insideCar = true;
			Debug.Log("insideCar = true");
			
			// on the car!
			transform.parent = screetCarToTake.transform;
		}
		


		if (insideCar && !firstTurn) {
//			Debug.Log("prepare to turn!");

			//turn left
//			if(anim.IsInTransition (0) && anim.GetNextAnimatorStateInfo (0).shortNameHash == turnleftHash) {
			if(anim.GetNextAnimatorStateInfo (0).shortNameHash == turnleftHash) {
				firstTurn = true;
				LeanTween.rotateY (transform.gameObject, 180, 1f).setEase (LeanTweenType.linear);
				LeanTween.moveLocalZ (transform.gameObject, transform.localPosition.z-15, 2.5f).setEase (LeanTweenType.linear).setDelay(1f).setOnComplete ( TurnToFront );
				Debug.Log ("turn left!");
			}
			//turn right
//			else if(anim.IsInTransition (0) && anim.GetNextAnimatorStateInfo(0).shortNameHash == turnrightHash){
			else if(anim.GetNextAnimatorStateInfo(0).shortNameHash == turnrightHash){
				firstTurn = true;
				LeanTween.rotateY (transform.gameObject, 360, 1f).setEase(LeanTweenType.linear);
				LeanTween.moveLocalZ (transform.gameObject, transform.localPosition.z+15, 2.5f).setEase (LeanTweenType.linear).setDelay(1f).setOnComplete ( TurnToFront );
				Debug.Log("turn right!");
			}
		}

//		if(!firstTurn && anim.IsInTransition(0) && anim.GetNextAnimatorStateInfo(0).shortNameHash == turnrightHash){
//			firstTurn = true;
//			Debug.Log("firstTurn = true");
//			
//			LeanTween.rotateY( transform.gameObject, 180, 1f).setEase(LeanTweenType.linear);
//			Debug.Log("turn right!");
//			
//			if (getInFromLeftDoor) {
//				LeanTween.rotateY( transform.gameObject, 360, 1f).setEase(LeanTweenType.linear);
//				LeanTween.rotateY( transform.gameObject, 360, 1f).setEase(LeanTweenType.linear);
//				Debug.Log("turn left and!");
//			} else {
//				
//			}
//		}

		//for debugging
//		if(anim.IsInTransition(0)){
//			aniInfos = anim.GetNextAnimatorClipInfo(0);
//			for(int i=0; i<aniInfos.Length; i++){
//				Debug.Log(aniInfos[i].clip.name);
//			}
//		}
		
	}

	void TurnToFront(){
		anim.SetBool("FindSeat", true);
		anim.SetTrigger ("WalkStop");
		Debug.Log ("p: find seat, walk Stop");
		LeanTween.rotateY( transform.gameObject, 270, 1f).setEase(LeanTweenType.linear).setDelay(1f);
		LeanTween.moveLocalX (transform.gameObject, transform.localPosition.x+0.5f, 1f).setEase (LeanTweenType.linear).setDelay(1f).setOnComplete ( CarCanGo );
	}

	void CarCanGo(){
		pg.carShouldStopForP = false;
		Debug.Log ("p: car can go!");
	}
}
