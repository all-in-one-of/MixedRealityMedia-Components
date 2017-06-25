using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour {
	
	public MenuHintPresenter menuHintPresenter;
	public Canvas configCanvas; // might get replaced with ConfigPresenter ...

	void Start() {
		SetDefaultState();
	}

	void Update () {
		if(Input.anyKeyDown) {
			menuHintPresenter.Enable();
		}
	}

	void SetDefaultState() {
		menuHintPresenter.gameObject.SetActive(false);
		menuHintPresenter.onClick += () => { MenuHintClicked(); };
		// configCanvas.gameObject.SetActive(false);
	}

	void MenuHintClicked() {
		configCanvas.gameObject.SetActive(true);
	}
}
