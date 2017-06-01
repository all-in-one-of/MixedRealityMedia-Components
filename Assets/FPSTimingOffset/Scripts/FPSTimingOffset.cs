using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSTimingOffset : MonoBehaviour {

	[RangeAttribute(0.1f, 120)]
	public float renderFPS;	
	[TooltipAttribute("in ms")]
	public float timeOffset;
	public Camera observerCamera;
	public Material displayMaterial;

	private RenderTexture firstBuffer;
	private RenderTexture secondBuffer;

	private float FrameTime {
		get { return 1000f / renderFPS; }
	}
	private float internalTimer;
	private bool first;

	void Start () {
		firstBuffer = new RenderTexture(Screen.width, Screen.height, 32, RenderTextureFormat.ARGB32);
		secondBuffer = new RenderTexture(Screen.width, Screen.height, 32, RenderTextureFormat.ARGB32);
		firstBuffer.antiAliasing = QualitySettings.antiAliasing;
		secondBuffer.antiAliasing = QualitySettings.antiAliasing;
		internalTimer = 0f;
		first = true;
	}
	
	void OnPreRender() {
		var renderTime = internalTimer + (Time.deltaTime * 1000);
		var overTime = renderTime - FrameTime - timeOffset;
		internalTimer += Time.deltaTime * 1000;
		if(overTime < 0) {
			return;
		}
		internalTimer = internalTimer % FrameTime + timeOffset;
		Debug.Log("Gotta swap dem buffers");
		if(first) {
			displayMaterial.mainTexture = firstBuffer;
			observerCamera.SetTargetBuffers(secondBuffer.colorBuffer, secondBuffer.depthBuffer);
		} else {
			displayMaterial.mainTexture = secondBuffer;
			observerCamera.SetTargetBuffers(firstBuffer.colorBuffer, firstBuffer.depthBuffer);
		}
		first = !first;
	}
}
