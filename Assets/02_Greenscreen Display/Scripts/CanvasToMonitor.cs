using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class CanvasToMonitor : MonoBehaviour {

	public Canvas mixedRealityCanvas;
	public string configFileName;
	public Config config;



	// Use this for initialization
	void Start () {
		var path = Path.Combine(Application.streamingAssetsPath, configFileName);
		if(!File.Exists(path)) {
			Debug.LogError("FATAL: No configuration file found.");
		}

		var json = File.ReadAllText(path);
		config = JsonUtility.FromJson<Config>(json);

		Display.displays[config.MainMonitor].Activate();
		mixedRealityCanvas.targetDisplay = config.MainMonitor;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	[Serializable]
	public class Config {
		public int MainMonitor;
		public int DebugMonitor;
	}
}
