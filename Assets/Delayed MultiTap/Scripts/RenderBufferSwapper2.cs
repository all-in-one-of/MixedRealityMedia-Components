using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RenderBufferSwapper2 : MonoBehaviour {
	[RangeAttribute(0, 30)]
	public int frameDelay;
	private List<RenderTexture> rgbaBuffers;
	private List<RenderTexture> depthBuffers;

	public int index;
	public long seenFrames;
	public Camera observerCamera;
	public bool inChromaShader;
	public Material cameraViewMaterial;
	public Material depthViewMaterial;

	void Start () {
		rgbaBuffers = new List<RenderTexture>();
		depthBuffers = new List<RenderTexture>();
		RebuildRenderBuffers();
		observerCamera.depthTextureMode = DepthTextureMode.Depth;
		Camera.onPreRender += OnPreRenderCB;
	}
	
	void Update () {
		if(frameDelay != rgbaBuffers.Count || frameDelay != depthBuffers.Count) {
			Debug.Log("I need to rebuild those g'damn buffers.");
			RebuildRenderBuffers();
		}
	}

	void OnPreRenderCB(Camera cam) {
		if(cam != observerCamera) {
			return;
		}
		index = index % frameDelay;
		observerCamera.SetTargetBuffers(
			rgbaBuffers[index].colorBuffer,
			depthBuffers[index].depthBuffer
		);
		var oldestFrame = rgbaBuffers[(index + 1) % frameDelay];
		var oldestDepth = depthBuffers[(index + 1) % frameDelay];
		if(inChromaShader) {
			cameraViewMaterial.SetTexture("_FrameTexture", oldestFrame);
			cameraViewMaterial.SetTexture("_DepthTexture", oldestDepth);
		} else {
			cameraViewMaterial.mainTexture = oldestFrame;
			if(depthViewMaterial != null) {
				depthViewMaterial.mainTexture = oldestDepth;
			}
		}
		// cameraViewMaterial.SetBuffer("_DepthTexture", oldestFrame.depthBuffer);
		index++;
		seenFrames++;
	}

	void RebuildRenderBuffers() {
		rgbaBuffers.RemoveAll(_ => true);
		depthBuffers.RemoveAll(_ => true);
		for(int i = 0; i < frameDelay; i++) {
			rgbaBuffers.Add(
				new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32)
			);
			depthBuffers.Add(
				new RenderTexture(Screen.width, Screen.height, 32, RenderTextureFormat.Depth)
			);
		}
		Debug.Log("Rebuilt buffers");
	}
}
