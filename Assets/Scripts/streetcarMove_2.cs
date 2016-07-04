using UnityEngine;
using System.Collections;

public class streetcarMove_2 : MonoBehaviour {

	public Transform[] trans;
	LTSpline cr;
	public LTDescr carMoving;
	public bool carStop = true;
	bool carStopBefore = false;

	Vector3 position = new Vector3(0,0,0);

	float elapsed = .0f;
	Vector3 originalPos;

	public P_Generator pg;

	public GameObject[] doors;	//R:0,2; L:1,3

	//-----------------------------------------------------------------------
	void Start () {
		originalPos = transform.position;

		// create the path
		cr = new LTSpline( new Vector3[] {
			trans[0].position, trans[1].position, trans[2].position, trans[3].position, 
			trans[4].position, trans[5].position, trans[6].position, trans[7].position,
			trans[8].position, trans[9].position, trans[10].position, trans[11].position, trans[12].position } );

		carMoving = LeanTween.moveSpline (transform.gameObject, cr.pts, 60f).setOrientToPath (true).setRepeat (-1).setDelay (4f);
//		carMoving = LeanTween.moveSpline (transform.gameObject, cr.pts, 60f).setOrientToPath (true).setRepeat (-1);
	}
	

	void Update () {
		/*
		if (!carStopBefore && pg.carShouldStopForP) {
			carMoving.pause ();
			carStopBefore = true;

			//open door
//			for(int i=0; i<doors.Length; i++){
//				if(i%2==0){
//					LeanTween.moveLocalX(doors[i].transform.gameObject, 6f, 2f).setEase (LeanTweenType.easeInOutBack);
//				}else{
//					LeanTween.moveLocalX(doors[i].transform.gameObject, -6f, 2f).setEase (LeanTweenType.easeInOutBack);
//				}
//			}
		}

		if (carStopBefore && !pg.carShouldStopForP) {
			carMoving.resume();
			carStopBefore = false;

			//close door
//			for(int i=0; i<doors.Length; i++){
//				LeanTween.moveLocalX(doors[i].transform.gameObject, 0f, 2f).setEase (LeanTweenType.easeInOutBack).setOnComplete(CarResumeMoving);
//			}
		}
		*/
	}

}
