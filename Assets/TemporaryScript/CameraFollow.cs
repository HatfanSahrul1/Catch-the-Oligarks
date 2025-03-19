using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	
	public float dampTime = 0.15f;
	public Transform target;
	[SerializeField] float yPlayer = 1.0f;
	void Update () 
	{
		if (target)
		{
			Vector3 from = transform.position;
			
			Vector3 playerPos = new Vector3(target.position.x, target.position.y + yPlayer, target.position.z);
			playerPos.z = transform.position.z;

			transform.position -= (from-playerPos)*dampTime*Time.deltaTime;
		}
	}
}