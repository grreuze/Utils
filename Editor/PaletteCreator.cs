using UnityEngine;
using UnityEditor;
using System.IO;

public class PaletteCreator : EditorWindow {

	string texName = "Textures/Palette";
	[SerializeField]
	Gradient _Gradient = new Gradient();
	[SerializeField]
	int _TextureSize = 128;
	[SerializeField]
	TextureWrapMode _WrapMode;
	[SerializeField]
	FilterMode _FilterMode;
	
	[MenuItem("Tools/Create Palette")]
	public static void ShowWindow() {
		GetWindow(typeof(PaletteCreator));
		GetWindow<PaletteCreator>().Show();
	}

	void OnGUI() {
		EditorGUILayout.Space();
		Event e = Event.current;
		EditorGUILayout.LabelField("Create Palette from Gradient:", EditorStyles.boldLabel);

		EditorGUILayout.Space();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Texture Name: Assets/", GUILayout.Width(146));
		texName = GUILayout.TextField(texName);
		EditorGUILayout.EndHorizontal();

		SerializedObject sO = new SerializedObject(this);
		SerializedProperty gradient = sO.FindProperty("_Gradient");
		SerializedProperty size = sO.FindProperty("_TextureSize");
		SerializedProperty wrapMode = sO.FindProperty("_WrapMode");
		SerializedProperty filterMode = sO.FindProperty("_FilterMode");

		EditorGUI.BeginChangeCheck();

			EditorGUILayout.PropertyField(gradient, true);

			EditorGUILayout.Space();

			EditorGUILayout.PropertyField(size, true);
			EditorGUILayout.PropertyField(wrapMode, true);
			EditorGUILayout.PropertyField(filterMode, true);

		if (EditorGUI.EndChangeCheck()) {
			sO.ApplyModifiedProperties();
		}
		
		EditorGUILayout.Space();
		if (GUILayout.Button("Create Palette") || e.isKey && e.keyCode == KeyCode.Return) {
			CreatePalette();
			Close();
		}
		Repaint();
	}

	void CreatePalette() {
		Texture2D texGradient = new Texture2D(_TextureSize, 1);
		texGradient.filterMode = _FilterMode;
		texGradient.wrapMode = TextureWrapMode.Clamp;
		texGradient.alphaIsTransparency = true;

		for (int i = 0; i < _TextureSize; i++) {
			float time = (float)i / _TextureSize;
			Color value = _Gradient.Evaluate(time);
			texGradient.SetPixel(i, 1, value);
		}

		texGradient.Apply();
		
		byte[] bytes = texGradient.EncodeToPNG();
		File.WriteAllBytes(Application.dataPath + "/" + texName + ".png", bytes);
		AssetDatabase.Refresh();
		DestroyImmediate(texGradient);
	}

}
