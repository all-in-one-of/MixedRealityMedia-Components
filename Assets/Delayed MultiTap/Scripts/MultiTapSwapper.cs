using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class MultiTapSwapper : MonoBehaviour {
#region Public Attributes
	[RangeAttribute(0, 30)]
	public int frameDelay;
	public int index;
	public long seenFrames;
	public Camera observerCamera;
	public bool inChromaShader;
	public Material cameraViewMaterial;
	public Material depthViewMaterial;
#endregion
#region Private Attributes
	private List<RenderTexture> rgbaBuffers;
	private List<RenderTexture> depthBuffers;
	private List<Vector4>		projectionParams;
#endregion

	void Start () {
		seenFrames = 0;
		rgbaBuffers = new List<RenderTexture>();
		depthBuffers = new List<RenderTexture>();
		projectionParams = new List<Vector4>();
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
		projectionParams[index] = GetProjectionParameter();

		var oldestIndex = (index + 1) % frameDelay;
		var oldestFrame = rgbaBuffers[oldestIndex];
		var oldestDepth = depthBuffers[oldestIndex];
		var oldestProjection = projectionParams[oldestIndex];
		if(inChromaShader) {
			cameraViewMaterial.SetTexture("_FrameTexture", oldestFrame);
			cameraViewMaterial.SetTexture("_DepthTexture", oldestDepth);
		} else {
			cameraViewMaterial.mainTexture = oldestFrame;
			if(depthViewMaterial != null) {
				depthViewMaterial.mainTexture = oldestDepth;
				depthViewMaterial.SetVector("_CustomProjection", oldestProjection);
			}
		}
		// cameraViewMaterial.SetBuffer("_DepthTexture", oldestFrame.depthBuffer);
		index++;
		seenFrames++;
	}

	void RebuildRenderBuffers() {
		rgbaBuffers.RemoveAll(_ => true);
		depthBuffers.RemoveAll(_ => true);
		projectionParams.RemoveAll(_ => true);
		for(int i = 0; i < frameDelay; i++) {
			rgbaBuffers.Add(
				new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32)
			);
			depthBuffers.Add(
				new RenderTexture(Screen.width, Screen.height, 32, RenderTextureFormat.Depth)
			);
			projectionParams.Add(GetProjectionParameter());
		}
		Debug.Log("Rebuilt buffers");
	}

	Vector4 GetProjectionParameter() {
		// From UnityShaderVariables.cginc:
		// x = 1 (D3D) or -1 (OpenGL)
		// y = near Plane
		// z = far Plane
		// w = 1 / far Plane

		// set platform code
		float platform;
		switch(SystemInfo.graphicsDeviceType) {
			case GraphicsDeviceType.Direct3D12:
			case GraphicsDeviceType.Direct3D11:
			case GraphicsDeviceType.Direct3D9:
			case GraphicsDeviceType.Metal: // seems to be simliar in projection
				platform = 1;
				break;
			case GraphicsDeviceType.OpenGLES2:
			case GraphicsDeviceType.OpenGLES3:
			case GraphicsDeviceType.OpenGLCore:
			case GraphicsDeviceType.Vulkan:
				platform = -1;
				break;
			default:
				platform = 0;
				break;
		}

		// Assemble projection vector
		return new Vector4(
			platform,
			observerCamera.nearClipPlane, observerCamera.farClipPlane,
			1 / observerCamera.farClipPlane
		);
	}
}
