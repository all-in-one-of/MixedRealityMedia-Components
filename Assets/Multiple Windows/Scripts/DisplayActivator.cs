using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayActivator : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log("Displays connected: " + Display.displays.Length);
		for(int i = 1; i < Display.displays.Length; i++) {
			Display.displays[i].Activate();
		}
	}
}
