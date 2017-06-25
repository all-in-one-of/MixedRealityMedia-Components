using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RenderSwapperController : MonoBehaviour {

	public Slider framerate;
	public Slider offset;
	public Button button;

	public delegate void OnValueChange(int framerate, float offset);
	public OnValueChange onValueChange;

	void Start () {
		if(!framerate || !offset || !button) {
			Debug.LogError("Either Framerate/Offset Slider or Set-Button missing.");
			return;
		}

		button.onClick.AddListener(() => {
			button.interactable = false;
			if(onValueChange != null) {
				onValueChange((int) framerate.value, offset.value);
			}
		});
		framerate.onValueChanged.AddListener((value) => {ValueChange();});
		offset.onValueChanged.AddListener((value) => {ValueChange();});
	}

	void ValueChange() {
		button.interactable = true;
	}
}
