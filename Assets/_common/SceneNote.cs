using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SceneNote : MonoBehaviour {
	[TextArea(10, 3)]
	public string note;
}
