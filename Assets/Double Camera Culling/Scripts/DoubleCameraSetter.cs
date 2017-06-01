using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DoubleCameraSetter : MonoBehaviour {

	[TooltipAttribute("Takes configuration lead, upon start " +
		"configuration of farCamera will be overwritten by " +
		"configuration of nearCamera")]
	public Camera nearCamera;
	public Camera farCamera;
	public float splitDepth;
	[TooltipAttribute("Lets near/far plane overlap each other for a" + 
		" specific margin.")]
	public float depthOverlap;
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
		// sync projection mode
		farCamera.orthographic = nearCamera.orthographic;
		// set correct render order
		nearCamera.depth = farCamera.depth + 1;
		// near camera only clears depth buffer
		nearCamera.clearFlags = CameraClearFlags.Depth;
	}
	
	void Update () {
		// If the overlap is too small/big, one camera will not render properly.
		var minSplitDist = Mathf.Max(nearFarPlane.x, depthOverlap) + 0.001f;
		var maxSplitDist = Mathf.Max(nearFarPlane.y, depthOverlap) - 0.001f; 
		splitDepth = Mathf.Clamp(splitDepth, minSplitDist, maxSplitDist);
		// sync cameras
		farCamera.transform.position = nearCamera.transform.position;
		// set clip planes correctly
		nearCamera.nearClipPlane = nearFarPlane.x;
		nearCamera.farClipPlane = splitDepth + depthOverlap;
		farCamera.nearClipPlane = splitDepth - depthOverlap;
		farCamera.farClipPlane = nearFarPlane.y;
	}
}
