using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour 
{
	public static CameraFollow Instance;	
	public float dampTime = 0.15f;
	public Transform target;
	[SerializeField] float yPlayer = 1.0f;
	[SerializeField] float yHome = 1.0f;
	[SerializeField] bool isHome = false;

	void Awake(){
		if (Instance==null) Instance = this;
	}
	void Update () 
	{
		if (target)
		{
			Vector3 from = transform.position;
			
			Vector3 playerPos = new Vector3(target.position.x, (isHome) ? yHome : yPlayer, target.position.z);
			playerPos.z = transform.position.z;

			transform.position -= (from-playerPos)*dampTime*Time.deltaTime;
		}
	}
}