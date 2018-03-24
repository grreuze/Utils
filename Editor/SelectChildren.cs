using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SelectChildren : EditorWindow {
	
	[MenuItem("GameObject/Select Children %<", false)]
	public static void SelectTheChildren() {
		if (Selection.gameObjects.Length > 0) {
			List<GameObject> children = new List<GameObject>();
			foreach (GameObject go in Selection.gameObjects) {
				foreach (Transform child in go.transform)
					children.Add(child.gameObject);
			}
			Selection.objects = children.ToArray();
		}
	}
}
