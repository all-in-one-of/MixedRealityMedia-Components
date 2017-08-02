using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitApplicationButton : MonoBehaviour {
	public Button quitButton;

	// Use this for initialization
	void Start () {
		if(!quitButton) {
			Debug.LogError("Quit button not set.");
			return;
		}	
		
		quitButton.onClick.AddListener(() => {
			Application.Quit();
		});
	}
	
}
