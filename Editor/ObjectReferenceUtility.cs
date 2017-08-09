using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class ObjectReferenceUtility : EditorWindow {


	private Component component;
	private UnityEngine.Object replacement;
	bool specificComponent = false;
		
	[MenuItem("GameObject/Find or Replace References", priority = 40)]
	public static void Init() {
		GetWindow(typeof(ObjectReferenceUtility));
		GetWindow<ObjectReferenceUtility>().Show();
	}

	public void OnGUI() {
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Find References to selected GameObject:", EditorStyles.boldLabel);
		EditorGUILayout.Space();
		
		if (specificComponent = GUILayout.Toggle(specificComponent, "Single Component"))
			component = EditorGUILayout.ObjectField("Component: ", component, typeof(Component), true) as Component;

		EditorGUILayout.Space();

		EditorGUI.BeginDisabledGroup(specificComponent ? component == null : Selection.gameObjects.Length == 0);
		if (GUILayout.Button("Display References in Console", EditorStyles.miniButton)) {
			if (specificComponent) {
				FindObjectsReferencing(component);
			} else {
				GameObject target = Selection.gameObjects[0];
				FindAllReferences(target);
			}
		}
		EditorGUI.EndDisabledGroup();

		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Replace All References with:", EditorStyles.boldLabel);
		EditorGUILayout.Space();
		
		replacement = EditorGUILayout.ObjectField(specificComponent ? "Component:" : "GameObject:", replacement, specificComponent && component? component.GetType() : typeof(GameObject), true);

		EditorGUILayout.Space();

		EditorGUI.BeginDisabledGroup(replacement && specificComponent ? component == null : Selection.gameObjects.Length == 0);
		if (GUILayout.Button("Replace References")) {
			if (specificComponent) {
				ReplaceReferences(component, replacement);
			} else {
				GameObject target = Selection.gameObjects[0];
				ReplaceAllReferences(target, (GameObject)replacement);
			}
		}
		EditorGUI.EndDisabledGroup();
		Repaint();
	}

	public static void FindAllReferences(GameObject go) {
		FindObjectsReferencing(go);
		foreach (Component c in go.GetComponents(typeof(Component))) {
			FindObjectsReferencing(c);
		}
	}
	
	public static void ReplaceAllReferences(GameObject source, GameObject target) {
		ReplaceReferences(source, target);
		foreach (Component c in source.GetComponents(typeof(Component)))
			ReplaceReferences(c, target.GetComponent(c.GetType()));
	}

	#region Private Methods

	private static void FindObjectsReferencing<T>(T mb) where T : UnityEngine.Object {
		var objs = Resources.FindObjectsOfTypeAll(typeof(Component)) as Component[];

		if (objs == null) return;
		foreach (Component obj in objs) {
			FieldInfo[] fields =
				obj.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance |
										BindingFlags.Static);
			foreach (FieldInfo fieldInfo in fields) {
				if (FieldReferencesComponent(obj, fieldInfo, mb)) {
					Debug.Log("Ref: Component " + obj.GetType() + " from Object " + obj.name + " references source component " + mb.GetType(), obj.gameObject);
				}
			}
		}
	}

	private static bool FieldReferencesComponent<T>(Component obj, FieldInfo fieldInfo, T mb) where T : UnityEngine.Object {
		if (fieldInfo.FieldType.IsArray) {
			var arr = fieldInfo.GetValue(obj) as Array;
			if (arr == null) return false;
			foreach (object elem in arr) {
				if (elem != null && mb != null && elem.GetType() == mb.GetType()) {
					var o = elem as T;
					if (o == mb)
						return true;
				}
			}
		} else {
			if (fieldInfo.FieldType == mb.GetType()) {

				var o = fieldInfo.GetValue(obj) as T;
				if (o == mb)
					return true;
			}
		}
		return false;
	}

	private static void ReplaceReferences<T>(T mb, T target) where T : UnityEngine.Object {
		var objs = (Component[])Resources.FindObjectsOfTypeAll(typeof(Component));

		if (objs == null) return;
		foreach (Component obj in objs) {
			FieldInfo[] fields =
				obj.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance |
										BindingFlags.Static);
			foreach (FieldInfo fieldInfo in fields)
				ReplaceReferenceInField(obj, fieldInfo, mb, target);
		}
	}

	private static void ReplaceReferenceInField<T>(Component obj, FieldInfo fieldInfo, T mb, T target) where T : UnityEngine.Object {
		if (fieldInfo.FieldType.IsArray) {
			var arr = (Array)fieldInfo.GetValue(obj);
			for (int i = 0; i < arr?.Length; i++) {
				object elem = arr.GetValue(i);
				if (elem != null && mb != null && elem.GetType() == mb.GetType()) {
					var o = (T)elem;
					if (o == mb)
						arr.SetValue(target, i);
				}
			}
		} else {
			if (fieldInfo.FieldType == mb.GetType()) {
				var o = (T)fieldInfo.GetValue(obj);
				if (o == mb)
					fieldInfo.SetValue(obj, target);
			}
		}
	}
	#endregion
}
