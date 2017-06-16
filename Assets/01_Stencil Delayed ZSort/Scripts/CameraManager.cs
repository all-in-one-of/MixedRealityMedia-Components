using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

	public List<Camera> managedCameras;
	public enum LensType {
		RectliniarFilm,
		RectliniarDSLR
	}
	public LensType type = LensType.RectliniarFilm;
	public float focalLength;
	[HideInInspector, SerializeField]
	public float FocalLength {
		get {
			return focalLength;
		}
		set {
			focalLength = value;
			RecalculateFOV();
		}
	}
	public Vector2 SensorSize {
		get {
			return sensorSize;
		}
		set {
			sensorSize = value;
			RecalculateFOV();
		}
	}
	[HideInInspector, SerializeField]
	public Vector2 sensorSize;
	public float fieldOfView;
	public Transform rootTransform;
	public Transform controllerTransform;

	void Start () {
		RecalculateFOV();
	}

	void Update() {
		rootTransform.position = controllerTransform.position;
		rootTransform.rotation = controllerTransform.rotation;
	}
	
	private void RecalculateFOV() {
		double fovdub = Mathf.Rad2Deg * 2.0 * Mathf.Atan(sensorSize.y  / (2f * focalLength));
		fieldOfView = (float) fovdub;
		foreach(Camera c in managedCameras) {
			c.fieldOfView = fieldOfView;
		}
	}
}
