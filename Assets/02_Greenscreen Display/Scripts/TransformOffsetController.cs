using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TransformOffsetController : MonoBehaviour {

	public Slider posX;
	public Slider posY;
	public Slider posZ;

	public Slider rotX;
	public Slider rotY;
	public Slider rotZ;


	List<Slider> ControlElements {
		get {
			return new List<Slider>(
				new Slider[]{posX, posY, posZ, rotX, rotY, rotZ	}
			);
		}
	}

	public delegate void OnValueChange(Vector3 position, Quaternion rotation);
	public OnValueChange onValueChange;

	void Start () {
		if(ControlElements.Any((x) => {return x == null;})) {
			Debug.LogError("At least on Camera Offset Slider is missing!");
			return;
		}
		foreach(var slider in ControlElements) {
			slider.onValueChanged.AddListener((value) => { ValueChanged(); });
		}
	}
	
	void ValueChanged() {
		if(onValueChange == null) {
			return;
		}

		var pos = new Vector3(posX.value, posY.value, posZ.value);
		var rot = Quaternion.Euler(rotX.value, rotY.value, rotZ.value);
		onValueChange(pos, rot);
	}
}
