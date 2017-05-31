using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TextMesh))]
public class FrameCounter3D : MonoBehaviour {
	public string pretext;
	private TextMesh mesh;

	void Start() {
		mesh = GetComponent<TextMesh>();
	}		

	void Update() {
		mesh.text = pretext + (Time.frameCount / 10 % 60);
	}
}
