using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TransformManager : MonoBehaviour {

	private static TransformManager instance;
	public static TransformManager Instance {
		get {
			return GetInstance();
		}
	}

	private Dictionary<string, Transform> tfStorage;

	void Start () {
		if(instance != null) {
			Debug.LogWarning("This instance will delete itself.");
			Component.Destroy(this);
			return;
		}

		instance = this;
		tfStorage = new Dictionary<string, Transform>();
	}

	public Transform GetTransform(string name) {
		return tfStorage[name];
	} 

	public void AddTransform(string name, Transform transform, bool force=false) {
		if(!tfStorage.ContainsKey(name)) {
			tfStorage[name] = transform;
			return;
		}

		if(force) {
			tfStorage[name] = transform;
			Debug.LogWarning(
				"A key with name [" + name + "] was already added," + 
				" but it was force-overwritten."
			);
			return;
		}
		Debug.LogWarning(
			"A key named [" + name + "] already existed and" + 
			" was not force-overwritten."
		);
	}

	private static TransformManager GetInstance() {
		if(instance != null) {
			return instance;
		}

		var go = new GameObject("Controller Transform Manager (generated)");
		var mngr = go.AddComponent<TransformManager>();
		return mngr;
	}
}
