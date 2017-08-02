using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigCanvasController : MonoBehaviour {

	public TransformOffsetController tOController;
	public RenderModeController rMController;
	public RenderSwapperController rSController;
	public CameraSettingsController cSController;


	public TransformOffsetController.OnValueChange cameraOffsetChange;
	public RenderModeController.OnValueChange rendermodeChange;
	public RenderSwapperController.OnValueChange renderSwapperChange;
	public CameraSettingsController.OnValueChange cameraSettingsChange;

	void Start() {
		tOController.onValueChange += (pos, rot) => {
			if(cameraOffsetChange != null) {
				cameraOffsetChange(pos, rot);
			}
		};

		rMController.onValueChange += (val) => {
			if(rendermodeChange != null) {
				rendermodeChange(val);
			}
		};

		rSController.onValueChange += (fps, offset) => {
			if(renderSwapperChange != null) {
				renderSwapperChange(fps, offset);
			}
		};

		cSController.onValueChange += (width, height, focalLength, camName) => {
			if(cameraSettingsChange != null) {
				cameraSettingsChange(width, height, focalLength, camName);
			}
		};
	}
}
