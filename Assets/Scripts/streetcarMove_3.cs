using UnityEngine;
using System.Collections;

public class streetcarMove_3 : MonoBehaviour {
	// the original/old one

	public Transform[] trans;
	LTSpline cr;
	LTDescr carMoving;
	public bool carStop = false;
	bool carStopBefore = false;

	Vector3 position = new Vector3(0,0,0);

	float elapsed = .0f;
	Vector3 originalPos;

	//stop
	public GameObject stopMarker;
	public P_Generator pg;
	bool onTheStopMarker = false;
	public GameObject[] doors;	//R:0,2; L:1,3

	bool touchMarkerBefore = false;
	bool touchMarker = false;

	public GameObject streetcar2;
	streetcarMove_2 s_c_m_2;

	float distChanging = 2f;
	//
	IEnumerator distCoroutine;

	//
//	public GameObject camShaker;
//	public CamShaker camSs;

	//-----------------------------------------------------------------------
	void Awake () {
		
	}
	//-----------------------------------------------------------------------
	void Start () {
		originalPos = transform.position;

		// create the path
		cr = new LTSpline( new Vector3[] {
			trans[0].position, trans[1].position, trans[2].position, trans[3].position, 
			trans[4].position, trans[5].position, trans[6].position, trans[7].position,
			trans[8].position, trans[9].position, trans[10].position, trans[11].position, trans[12].position } );

		carMoving = LeanTween.moveSpline(transform.gameObject, cr.pts, 60f).setOrientToPath(true).setRepeat(-1);

		//
		s_c_m_2 = streetcar2.GetComponent<streetcarMove_2>();

		// set CamShaker as child
//		camShaker.transform.parent = transform;
//		camShaker.transform.SetParent( transform );
//		camSs.originalCamPos = camShaker.transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
//		position.x += .1f;
//		transform.position = position;

		//stop
//		if (Input.GetKeyDown ("space") || transform.position == stopMarker.transform.position) {
//			if(!carStop){
//				carMoving.pause();
//				carStop = true;
//			}else{
//				carMoving.resume();
//				carStop = false;
//			}
//		}

		//v2
//		if (!carStopBefore && transform.position == stopMarker.transform.position) {
//			carMoving.pause ();
//			carStopBefore = true;
//		}
//		
//		if (!carStop && carStopBefore) {
//			carMoving.resume();
//			carStopBefore = false;
//		}


		//debug
//		if (transform.position == stopMarker.transform.position) {
//			Debug.Log("car: touch marker");
//		}

		//v3
		float dist = Vector3.Distance(stopMarker.transform.position, transform.position);

		if( !touchMarker && dist<distChanging ){
			touchMarker = true;
			Debug.Log("car: touch marker");
			pg.generateP = true;
		}

//		if ( !carStopBefore && (pg.carShouldStopForP || touchMarkerBefore) ) {
		if ( !carStopBefore && pg.carShouldStopForP ) {
			carMoving.pause ();
			carStop = true;
			carStopBefore = true;

//			if(transform.position == stopMarker.transform.position)
//				onTheStopMarker = true;

			//open door
			for(int i=0; i<doors.Length; i++){
				if(i%2==0){
					LeanTween.moveLocalX(doors[i].transform.gameObject, 13f, 2f).setEase (LeanTweenType.easeInOutBack);
				}else{
					LeanTween.moveLocalX(doors[i].transform.gameObject, -13f, 2f).setEase (LeanTweenType.easeInOutBack);
				}
			}

			Debug.Log("car: stop, doors opened");

			// 2nd car
			s_c_m_2.carMoving.pause();
		}

//		if ( carStopBefore && !pg.carShouldStopForP && onTheStopMarker ) {
		if ( carStopBefore && !pg.carShouldStopForP ) {
//			carMoving.resume();
			carStopBefore = false;
//			touchMarkerBefore = false;

//			if(onTheStopMarker)
//				onTheStopMarker = false;

			//close door, and do callback function "CarResumeMoving"
			for(int i=0; i<doors.Length; i++){
				if(i==doors.Length-1)
					LeanTween.moveLocalX(doors[i].transform.gameObject, 0f, 2f).setEase (LeanTweenType.easeInOutBack).setOnComplete(CarResumeMoving);
				else
					LeanTween.moveLocalX(doors[i].transform.gameObject, 0f, 2f).setEase (LeanTweenType.easeInOutBack);
			}
		}

//		if(transform.position == stopMarker.transform.position){
//			Debug.Log("stop marker");
//		}
	}

	void CarResumeMoving(){
		carMoving.resume();
		carStop = false;
		Debug.Log("car: go!");
		distChanging = 0f;

		distCoroutine = ChangeDist();
		StartCoroutine( distCoroutine );

		touchMarker = false;

		// 2nd car
		s_c_m_2.carMoving.resume();
	}

	IEnumerator ChangeDist() {
		yield return new WaitForSeconds(2f);
		distChanging = 2f;
		StopCoroutine( distCoroutine );
	}
	
}
