using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(WebcamEnabler))]
public class WebcamEnablerEditor : Editor {
	public override void OnInspectorGUI() {
		var tgt = target as WebcamEnabler;
		if(tgt == null) {
			GUILayout.Label("No suitable object attached.");
			return;
		}

		EditorGUILayout.ObjectField("Material", tgt.material, typeof(Material), true);
		tgt.imageSize = EditorGUILayout.Vector2Field("Resolution", tgt.imageSize);
		tgt.frameRate = EditorGUILayout.IntSlider("Framerate", tgt.frameRate, 1, 60);
        tgt.CamIndex = EditorGUILayout.Popup(
			"Webcam Selection",
            tgt.CamIndex,
            WebcamEnabler.CamNames
        );
        serializedObject.ApplyModifiedProperties();
		{
			EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button("Reload Cameras")) {
				WebcamEnabler.ReloadCameras();
			}
			if(GUILayout.Button("Stop Camera")) {
				tgt.StopCamera();
			}
			EditorGUILayout.EndHorizontal();
		}
	}
}