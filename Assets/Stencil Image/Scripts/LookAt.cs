using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour {

	public Transform lookAtWhat;
	
	void Update () {
		transform.LookAt(lookAtWhat);
	}
}
