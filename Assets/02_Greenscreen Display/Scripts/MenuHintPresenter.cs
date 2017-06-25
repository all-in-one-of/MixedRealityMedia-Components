using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Canvas))]
public class MenuHintPresenter : MonoBehaviour {
	
	private RectTransform pos;
	private bool animating = false;
	public Vector3 startPos;
	[Range(1, 300)]
	public int lerpFrames;
	[Range(0.5f, 10f)]
	public float menuHintDisplayDuration;
	public Button hintButton;

	public delegate void OnClick();
	public OnClick onClick;

	void Start() {
		pos = GetComponent<RectTransform>();
		if(!hintButton) {
			Debug.LogWarning("No clicks will be registered - Menu Hint button missing.");
		} else {
			hintButton.onClick.AddListener(() => {
				if(onClick != null) {
					onClick();
				}
			});
		}

	}
	
	public void Enable() {
		if(animating) {
			return;
		}
		// Somehow just started breaking - so we reload the rect transform
		pos = GetComponent<RectTransform>();
		animating = true;
		gameObject.SetActive(true);
		StartCoroutine(EnableCoroutine());
	}

	IEnumerator EnableCoroutine() {
		var destPos = pos.position;
		pos.position = pos.position - startPos;
		
		for(int i = 0; i < lerpFrames; i++) {
			pos.position = Vector3.Lerp(pos.position, destPos, ((float)i) / lerpFrames);
			yield return new WaitForEndOfFrame();
		}

		pos.position = destPos;
		yield return new WaitForSeconds(menuHintDisplayDuration);
		animating = false;
		gameObject.SetActive(false);
	}
}
