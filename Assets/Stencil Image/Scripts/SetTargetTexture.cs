using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTargetTexture : MonoBehaviour {

	public RenderTexture tex;

	// Use this for initialization
	void Start () {
		GetComponent<Camera>().targetTexture = tex;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
