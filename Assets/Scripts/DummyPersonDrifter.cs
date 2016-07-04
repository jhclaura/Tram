using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterController))]
public class DummyPersonDrifter: MonoBehaviour
{
    private Vector3 moveDirection = Vector3.zero;
    private Transform myTransform;
	//
	public GameObject headRotTarget;
	private Quaternion dummyHeadRotationRaw;
	private Quaternion dummyHeadRotation;
//	public Cardboard cardboardahh;
 

    void Start()
    {
        myTransform = transform;
    }
 
    void FixedUpdate()
	{
		//sync head's rotation.y
		dummyHeadRotationRaw = headRotTarget.transform.rotation;
		dummyHeadRotationRaw.x = dummyHeadRotationRaw.z = 0;
		dummyHeadRotation = NormalizeQ (dummyHeadRotationRaw);
		myTransform.rotation = dummyHeadRotation;
//		Debug.Log ("multiTouchEnabled: "+Input.multiTouchEnabled);
    }

	Quaternion NormalizeQ(Quaternion q)
	{
		Quaternion result;
		float sq = q.x * q.x;
		sq += q.y * q.y;
		sq += q.z * q.z;
		sq += q.w * q.w;
		//detect badness
//		assert(sq > 0.1f);
		float inv = 1.0f / Mathf.Sqrt(sq);
		result.x = q.x * inv;
		result.y = q.y * inv;
		result.z = q.z * inv;
		result.w = q.w * inv;
		return result;
	}
}