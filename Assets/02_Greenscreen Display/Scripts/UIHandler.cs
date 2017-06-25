using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour {

	public Canvas rootCanvas;
	public Canvas switchPanel;
	public Button yesButton;
	public Button noButton;
	private int targetDisplay;

	// Use this for initialization
	void Start () {
		targetDisplay = rootCanvas.targetDisplay;
		yesButton.onClick.AddListener(() => {
			switchPanel.enabled = false;
		});

		noButton.onClick.AddListener(() => {

			rootCanvas.targetDisplay++;

			var display = Display.displays[++targetDisplay];
			if(!display.active) {
				display.Activate();
			}
		});
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
