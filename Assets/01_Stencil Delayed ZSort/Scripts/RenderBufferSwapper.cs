using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class RenderBufferSwapper : MonoBehaviour {
    /*
    public List<RenderTexture> buffers;
	public int index;
	public long seenFrames;
	public Material cameraViewMaterial;
    public RawImage rawOutputDisplay;
    */ 
    [RangeAttribute(15, 60)]
    public float cameraFPS;
    [RangeAttribute(0, 1)]
    public float cameraOffset;
    public Material targetMaterial;
    public Camera fullCamera;
    public Camera frontCamera;
	public Camera stencilCamera;
    public Camera lightCamera;
	public string materialFullFieldName;
	public string materialFrontFieldName;
	public string materialWebcamFieldName;
    public string materialWebcamMaskFieldName;
    public string materialLightFieldName;


    private float frameWindow;
    private float delay;
    private int IntDelay { get { return (int)delay;} }
    private float frameDelay;
    private int IntFrameDelay { get { return (int)frameDelay;} }
    private float fractionDelay;
    private float innerTimer;
    private float absoluteTimer;
    private float initialDelay;
    public List<RenderTexture> colorBuffers;
    public List<RenderTexture> alphaBuffers;
	public List<RenderTexture> stencilBuffers;
    public List<RenderTexture> lightBuffers;
	public WebcamEnabler webcamEnabler;
    public int index;


    void Start () {
        frameWindow = 1.0f / cameraFPS;
        delay = cameraOffset / frameWindow;
        frameDelay = (int)delay * frameWindow;
        fractionDelay = delay % 1 * frameWindow;

        innerTimer = 0.0f;
        absoluteTimer = 0.0f;
        initialDelay = frameDelay + fractionDelay;

		RebuildRenderBuffers();

        targetMaterial.SetTexture(materialWebcamFieldName, webcamEnabler.webcamTexture);
	}
	
	void Update () {
        innerTimer += Time.deltaTime;
        absoluteTimer += Time.deltaTime;
        var localTime = innerTimer - fractionDelay;
        if(localTime < frameWindow || absoluteTimer < initialDelay) {
            return;
        }

        SwapRenderBuffer();
        innerTimer %= frameWindow;
        absoluteTimer = absoluteTimer % (2f) + initialDelay;
	}

    void SwapRenderBuffer() {
        index = index % IntDelay;
        //  fullCamera.SetTargetBuffers(colorBuffers[index].colorBuffer, depthBuffers[index].depthBuffer);
        fullCamera.targetTexture = colorBuffers[index];
        frontCamera.targetTexture = alphaBuffers[index];
        stencilCamera.targetTexture = stencilBuffers[index];
        lightCamera.targetTexture = lightBuffers[index];
        // stencilCamera.SetTargetBuffers(sBuf.colorBuffer, sBuf.depthBuffer);

		
        var frameTex = colorBuffers[(index + 1) % IntDelay];
        var alphaTex = alphaBuffers[(index + 1) % IntDelay];
		var stencilTex = stencilBuffers[(index + 1) % IntDelay];
        var lightTex = lightBuffers[(index + 1) % IntDelay];

		// at this point do the stencil operation on the WC material
        // stencilMaterial.mainTexture = stencilTex;
		// Graphics.Blit(stencilTex, stencilTex, stencilMaterial);
        // unlitDebugDisplay.mainTexture = stencilTex;
        targetMaterial.SetTexture(materialFullFieldName, frameTex);
        targetMaterial.SetTexture(materialFrontFieldName, alphaTex);
        targetMaterial.SetTexture(materialWebcamMaskFieldName, stencilTex);
        targetMaterial.SetTexture(materialLightFieldName, lightTex);
        index++;
    }

	void RebuildRenderBuffers() {
        colorBuffers = new List<RenderTexture>();
        alphaBuffers = new List<RenderTexture>();
		stencilBuffers = new List<RenderTexture>();
		for(int i = 0; i < IntDelay; i++) {
            var cBuf = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);
            cBuf.name = "Color Buffer " + i;
			// cBuf.antiAliasing = QualitySettings.antiAliasing;
            colorBuffers.Add(cBuf);


            var aBuf = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);
            aBuf.name = "Front Buffer " + i;
			// aBuf.antiAliasing = QualitySettings.antiAliasing;
            alphaBuffers.Add(aBuf);

			var sBuf = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);
			sBuf.name = "Stencil Buffer " + i;
			stencilBuffers.Add(sBuf);

            var lBuf = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);
            lBuf.name = "Light Buffer " + i;
            lightBuffers.Add(lBuf);
		}
		Debug.Log("Rebuilt buffers");
	}
}