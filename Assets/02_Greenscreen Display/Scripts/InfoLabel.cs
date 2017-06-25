using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Dropdown))]
public class InfoLabel : MonoBehaviour {

	public List<string> descriptions;
	public Text infoLabel;
	private Dropdown dropDown;

	void Start () {
		if(!infoLabel) {
			Debug.LogWarning("No text set, no additional information available to display.");
		}
		dropDown = GetComponent<Dropdown>();
		dropDown.onValueChanged.AddListener((value) => {
			if(value > descriptions.Count) {
				Debug.LogWarning("Selection higher than list count - there seems a description missing.");
				infoLabel.text = "";
				return;
			}
			infoLabel.text = descriptions[value];
		});

		infoLabel.text = descriptions.Count > 0 ? descriptions[0] : "";
	}
}