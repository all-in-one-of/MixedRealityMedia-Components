﻿using UnityEngine;

public class InputManager : MonoBehaviour {
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}
	}
}