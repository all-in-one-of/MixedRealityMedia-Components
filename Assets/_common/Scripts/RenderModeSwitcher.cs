using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RenderModeSwitcher : MonoBehaviour {


	// The major two render modes involve following steps:
	// - TriStep renders a front and a full color back, which then get
	//   placed into properly, this works best on simple render scenes
	// - BackPane renders a front alpha mask and a full back, where the 
	//   full back gets mixed with the camera alpha + front alpha.
	//   this produces a worse image reproduction, but usually works with all
	//   kinds of complicated render situations
	// Note: Deferred sets all cameras into Deferred mode

	public enum RenderMode {
		TriStepDeferredMode, TriStepForwardMode,
		BackpaneDeferredMode, BackpaneMode
	}
	[SerializeField,HideInInspector]
	private RenderMode _renderMode;

	public RenderMode CurrentRenderMode {
		get {
			return _renderMode;
		}
		set {
			_renderMode = value;
			ChangeMode();
		}
	}

	public Material triStepMaterial;
	public Material backpaneMaterial;

	public Material activeMaterial;
	public Camera frontCamera;


	void Start () {
		if(!triStepMaterial || !backpaneMaterial) {
			Debug.LogError("Tri Step Material or Backpane Material was not set");
		}
		if(!frontCamera) {
			var cams = GameObject.FindGameObjectsWithTag("Front Camera");
			if(cams.Length != 1) {
				Debug.LogError("No front camera set and none is found.");
			}
			frontCamera = cams[0].GetComponent<Camera>();
		}
		if(!frontCamera) {
			Debug.LogError(
				"GameObject tagged with 'Front Camera' " +
				"does not contain a camera."
			);
		}
	}
	
	void ChangeMode() {
		Debug.LogWarning("Render Mode change triggered - this might break some instances.");
		switch(_renderMode) {
			case RenderMode.TriStepForwardMode:
			case RenderMode.TriStepDeferredMode:
				activeMaterial = new Material(triStepMaterial);
				activeMaterial.CopyPropertiesFromMaterial(backpaneMaterial);
				break;
			case RenderMode.BackpaneMode:
			case RenderMode.BackpaneDeferredMode:
				activeMaterial = new Material(backpaneMaterial);
				activeMaterial.CopyPropertiesFromMaterial(triStepMaterial);
				break;
		}

		switch(_renderMode) {
			case RenderMode.TriStepDeferredMode:
			case RenderMode.BackpaneDeferredMode:
				frontCamera.renderingPath = RenderingPath.DeferredShading;
				break;
			case RenderMode.TriStepForwardMode:
			case RenderMode.BackpaneMode:
				frontCamera.renderingPath = RenderingPath.Forward;
				break;
		}
	}
}
