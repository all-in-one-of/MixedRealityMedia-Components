using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RenderBufferSwapper : MonoBehaviour {
	[RangeAttribute(0, 30)]
	public int frameDelay;
	public List<RenderTexture> buffers;

	public int index;
	public long seenFrames;
	public Camera observerCamera;
	public bool inChromaShader;
	public Material cameraViewMaterial;

	void Start () {
		RebuildRenderBuffers();
		observerCamera.depthTextureMode = DepthTextureMode.Depth;
		Camera.onPreRender += OnPreRender;
	}
	
	void Update () {
		if(frameDelay != buffers.Count) {
			Debug.Log("I need to rebuild those g'damn buffers.");
			RebuildRenderBuffers();
		}
	}

	void OnPreRender(Camera cam) {
		if(cam != observerCamera) {
			return;
		}
		index = index % frameDelay;
		observerCamera.targetTexture = buffers[index];
		var oldestFrame = buffers[(index + 1) % frameDelay];
		if(inChromaShader) {
			cameraViewMaterial.SetTexture("_FrameTexture", oldestFrame);
		} else {
			cameraViewMaterial.mainTexture = oldestFrame;
		}
		// cameraViewMaterial.SetBuffer("_DepthTexture", oldestFrame.depthBuffer);
		index++;
		seenFrames++;
	}

	void RebuildRenderBuffers() {
		buffers.RemoveAll(_ => true);
		for(int i = 0; i < frameDelay; i++) {
			buffers.Add(
				new RenderTexture(Screen.width, Screen.height, 32, RenderTextureFormat.ARGB32)
			);
		}
		Debug.Log("Rebuilt buffers");
	}
}
