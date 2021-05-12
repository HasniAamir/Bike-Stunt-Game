using UnityEngine;
using System.Collections;

public class CameraFollow2D : MonoBehaviour {
	
	public Transform target;
	
	private Transform cam;
	
	void Start()
	{
		cam = transform;
	}
	
	// Update is called once per frame
	void Update () {
	 cam.position = new Vector3( target.position.x, target.position.y, cam.position.z);
	}
}
