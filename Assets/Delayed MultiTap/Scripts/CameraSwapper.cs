using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwapper : MonoBehaviour {

	public List<Camera> cameras;
	private int index;

	void Update () {
		index = index % cameras.Count;
		for(int i = 0; i < cameras.Count; i++) {
			var cam = cameras[i];
			if(i == index) {
				cam.enabled = true;
				continue;
			}
			cam.enabled = false;
		}
		index ++;
	}
}
