using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraSettingsController : MonoBehaviour {

	public InputField width;
	public InputField height;
	public Slider focalLength;
	public Dropdown cameraNames;

	public delegate void OnValueChange(float width, float height, float focalLength, string cameraName);
	public OnValueChange onValueChange;

	void Start () {
		if(!width || !height || !focalLength || !cameraNames) {
			Debug.LogError(
				"Either Width/Height Inputfield, FocalLength Slider " + 
				"or CameraName Dropdown is missing."
			);
			return;
		}		
		width.onValueChanged.AddListener((value) => { ValueChange(); });
		height.onValueChanged.AddListener((value) => { ValueChange(); });
		focalLength.onValueChanged.AddListener((value) => { ValueChange(); });
		cameraNames.onValueChanged.AddListener((value) => { ValueChange(); });
	}

	void ValueChange() {
		if(onValueChange == null) {
			return;
		}
		var widthValue = float.Parse(width.text);
		var heightValue = float.Parse(height.text);
		var cameraName = cameraNames.options[cameraNames.value].text;
		onValueChange(widthValue, heightValue, focalLength.value, cameraName);
	}

	void SetCameraNames(List<string> names) {
		var list = new List<Dropdown.OptionData>();
		foreach(var name in names) {
			list.Add(new Dropdown.OptionData(name));
		}
		cameraNames.options = list;
	}
}
