using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseAction : MonoBehaviour {
	public Button closeButton;

	void Start () {
		if(!closeButton) {
			Debug.LogWarning("No close button - no close action can be set.");
			return;
		}
		
		closeButton.onClick.AddListener(() => {
			gameObject.SetActive(false);
		});
	}
}
