using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasPerMonitorSpawner : MonoBehaviour {

	public GameObject prefab;

	// Use this for initialization
	void Start () {
		for(int i = 0; i < Display.displays.Length; i++) {
			var display = Display.displays[i];
			display.Activate();
			var gameObject = Instantiate(prefab);
			var canvas = gameObject.GetComponent<Canvas>();
			canvas.targetDisplay = i;
			canvas.GetComponentInChildren<Text>().text = "Monitor\n#"+i;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}
	}
}
