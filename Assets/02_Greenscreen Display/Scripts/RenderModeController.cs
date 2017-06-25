using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RenderModeController : MonoBehaviour {

	public Dropdown dropdown;
	public delegate void OnValueChange(RenderModeSwitcher.RenderMode renderMode);
	public OnValueChange onValueChange;

	void Start () {
		dropdown.onValueChanged.AddListener((value) => {
			if(onValueChange == null) {
				return;
			}
			
			switch(value) {
				case 0:
					onValueChange(RenderModeSwitcher.RenderMode.MaskDeferredMode);
					break;
				case 1:
					onValueChange(RenderModeSwitcher.RenderMode.MaskForwardMode);
					break;
				case 2:
					onValueChange(RenderModeSwitcher.RenderMode.ReplaceDeferredMode);
					break;
				case 3:
					onValueChange(RenderModeSwitcher.RenderMode.ReplaceMode);
					break;
				default:
					Debug.LogError("Unexpected render mode selected.");
					break;
			}		
		});
	}
}
