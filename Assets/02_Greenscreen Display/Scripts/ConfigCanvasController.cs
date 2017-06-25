using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigCanvasController : MonoBehaviour {

	public TransformOffsetController tOController;
	public RenderModeController rMController;
	public RenderSwapperController rSController;

	public TransformOffsetController.OnValueChange cameraOffsetChange;
	public RenderModeController.OnValueChange rendermodeChange;
	public RenderSwapperController.OnValueChange renderSwapperChange;

	void Start() {
		// literally just a proxy.
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
	}
}
