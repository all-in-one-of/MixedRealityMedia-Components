using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MultiBufferCamera : MonoBehaviour {

	public RenderTexture depthTexture;
	public RenderTexture rgbaTexture;

	void OnEnable () {
		if(depthTexture == null) {
			Debug.LogError("No depth texture set");
			return;
		}
		if(rgbaTexture == null) {
			Debug.LogError("No RGBA texture set");
			return;
		}

		var cam = GetComponent<Camera>();
		if(cam == null) {
			Debug.LogError("Cannot find Camera on gameObject");
			return;
		}

		cam.SetTargetBuffers(rgbaTexture.colorBuffer, depthTexture.depthBuffer);
		Debug.Log("Finished setting up all buffers.");
	}
}
