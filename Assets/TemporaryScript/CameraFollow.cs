using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	
	public float dampTime = 0.15f;
	public Transform target;

	void Update () 
	{
		if (target)
		{
			Vector3 from = transform.position;
			Vector3 to = target.position;
			to.z = transform.position.z;

			transform.position -= (from-to)*dampTime*Time.deltaTime;
		}
	}
}