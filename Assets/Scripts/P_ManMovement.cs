using UnityEngine;
using System.Collections;

public class P_ManMovement : MonoBehaviour {

	Animator anim;
	int walkInHash = Animator.StringToHash("WalkIn");
	int insideStillHash = Animator.StringToHash("InsideStill");
	
	bool insideCar = false;
	
	public GameObject screetCarToTake;
	
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		
		//		Debug.Log ( standHash );
		//		Debug.Log ( anim.GetCurrentAnimatorStateInfo(0).shortNameHash );
	}
	
	// Update is called once per frame
	void Update () {
		
		if(!insideCar && anim.IsInTransition(0) && anim.GetNextAnimatorStateInfo(0).shortNameHash == insideStillHash){
			insideCar = true;
			Debug.Log("insideCar = true");
			
			// on the car!
			transform.parent = screetCarToTake.transform;
		}
		
		//		if(anim.IsInTransition(0)){
		//			Debug.Log(anim.GetNextAnimatorStateInfo(0).shortNameHash);
		//		}
		
	}
}
