using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleCameraSetter : MonoBehaviour {

	[TooltipAttribute("Takes configuration lead, upon start " +
		"configuration of farCamera will be overwritten by " +
		"configuration of nearCamera")]
	public Camera nearCamera;
	public Camera farCamera;
	public float splitDepth;
	public Vector2 nearFarPlane;
	
	void Start () {
		if(nearCamera == null) {
			Debug.LogError("Near camera missing.");
			enabled = false;
		}
		if(farCamera == null) {
			Debug.LogError("Far camera missing.");
			enabled = false;
		}
		
		farCamera.orthographic = nearCamera.orthographic;
		farCamera.depth = nearCamera.depth + 1;
	}
	
	void Update () {
		splitDepth = Mathf.Clamp(splitDepth, nearFarPlane.x, nearFarPlane.y);
		farCamera.transform.position = nearCamera.transform.position;
		nearCamera.nearClipPlane = nearFarPlane.x;
		nearCamera.farClipPlane = splitDepth;
		farCamera.nearClipPlane = splitDepth;
		farCamera.farClipPlane = nearFarPlane.y;
	}
}
