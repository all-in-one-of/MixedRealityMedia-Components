using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WebcamEnabler : MonoBehaviour {

	private static string[] _camNames;
	public static string[] CamNames {
		get {
			if(_camNames == null) {
				ReloadCameras();
			}
			return _camNames;
		}
	}
	public static void ReloadCameras() {
		List<string> container = new List<string>();
		WebCamTexture.devices.ToList().ForEach(x => {container.Add(x.name);});
		_camNames = container.ToArray();
	}

	[SerializeField]
	private int _camIndex;
	public int CamIndex {
		get {
			return _camIndex;
		}
		set {
			if(_camIndex != value) {
				_camIndex = value;
				deviceName = CamNames[value];
				ResetCamera();
			}
		}
	}
	public Material material;
	public string deviceName;

	public Vector2 imageSize;
	public int frameRate;

	private WebCamTexture wc;

	// Use this for initialization
	void Start () {
		if(imageSize.x == 0 && imageSize.y == 0) {
			imageSize = new Vector2(1280, 720);
		}
		ResetCamera();
	}

	void OnDisable() {
		if(wc != null && wc.isPlaying) {
			wc.Stop();
		}
	}

	void ResetCamera() {
		if(wc != null && wc.isPlaying) {
			wc.Stop();
		}
		if(!Application.isPlaying) {
			return;
		}
		Debug.Log("Restarting Camera: " + deviceName);
		wc = new WebCamTexture(deviceName, (int) imageSize.x, (int) imageSize.y, frameRate);
		material.mainTexture = wc;
		wc.Play();
	}

	public void StopCamera() {
		if(wc != null && wc.isPlaying) {
			wc.Stop();
		}
	}
}