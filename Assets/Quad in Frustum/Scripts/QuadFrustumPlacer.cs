using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadFrustumPlacer : MonoBehaviour {

	[Range(1, 100)]
	public float distance;
	public Transform follower;
	public GameObject plane;
	private Camera cam;
	

	void Start () {
		cam = GetComponent<Camera>();
		if(!cam) {
			Debug.LogError("This script is not attached to a camera.");
			return;
		}
		cam.nearClipPlane = 0.999f;
		cam.farClipPlane = 101f;
	}
	
	void Update () {
		var followerDistance = Vector3.Distance(cam.transform.position, follower.position);
		distance = Mathf.MoveTowards(distance, followerDistance, 3);
		var height = 2.0 * Mathf.Tan(0.5f * cam.fieldOfView * Mathf.Deg2Rad) * distance;
		var width = height * Screen.width / Screen.height;

		plane.transform.localScale = new Vector3((float) width / 10f, 1, (float) height / 10);
		plane.transform.localPosition = cam.transform.forward * distance;
	}
}
