using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StencilBlit : MonoBehaviour {

	public RenderTexture targetTexture;
	public RenderTexture stencilTarget;

	public Material simpleMaterial;
	public Material stencilMaterial;

	public RawImage display;

	private Camera cam;

	void Start() {
		cam = GetComponent<Camera>();
	}

	void OnRenderImage(RenderTexture src, RenderTexture dest) {
		cam.SetTargetBuffers(targetTexture.colorBuffer, targetTexture.depthBuffer);
		Graphics.Blit(targetTexture, targetTexture, stencilMaterial);
    }

	public void StencilBlitOperation(RenderTexture sourceTex, Material targetMaterial) {
		Graphics.Blit(sourceTex, sourceTex, targetMaterial);
	}
}
