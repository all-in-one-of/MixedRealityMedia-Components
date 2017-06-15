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

	void Start() {
/*
		targetTexture = new RenderTexture(Screen.width, Screen.height, 32);
		targetTexture.name = "Target Texture Thing";
		stencilTarget = new RenderTexture(Screen.width, Screen.height, 32);
		stencilTarget.name = "Stencil Target Thing";
*/
		GetComponent<Camera>().targetTexture = targetTexture;
// 		display.texture = targetTexture;
	}

	void OnRenderImage(RenderTexture src, RenderTexture dest) {
		// set a dummy buffer
		Graphics.SetRenderTarget(stencilTarget);
		// clear it
		GL.Clear(true, true, Color.black);

		// take dummy buffer and depth buffer of real camera
		Graphics.SetRenderTarget(stencilTarget.colorBuffer, targetTexture.depthBuffer);
		// blit the color into buffer with SimpleRender pass
		Graphics.Blit(targetTexture, simpleMaterial);
		// blit the color into buffer with PostprocessMaterial pass
		Graphics.Blit(targetTexture, stencilMaterial);
		// revoce all render textures (?)
		RenderTexture.active = null;
		// blit image to screen with simplerender 
		Graphics.Blit(stencilTarget, simpleMaterial);

//		RenderTexture.active = null;
//		Graphics.Blit(src, simpleMaterial);
        Graphics.Blit(src, dest);

    }
}
