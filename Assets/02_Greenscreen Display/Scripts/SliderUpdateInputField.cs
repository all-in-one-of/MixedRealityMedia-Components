using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderUpdateInputField : MonoBehaviour {
	public InputField inputField;
	public string formatString;
	public string postText;
	private Slider slider;
	// Use this for initialization
	void Start () {
		if(!inputField) {
			inputField = GetComponentInChildren<InputField>();
		}
		if(!inputField) {
			Debug.LogWarning("No Input Field found in children - maybe broken?");
		}
		slider = GetComponent<Slider>();
	}
	
	// Update is called once per frame
	void Update () {
		inputField.text = slider.value.ToString(formatString) + postText;
	}
}
