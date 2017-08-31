using UnityEngine;
using UnityEditor;
using System.IO;

public class PaletteCreator : EditorWindow {

	string texName = "Textures/Palette";

	[SerializeField] Gradient _Gradient = new Gradient();
	[SerializeField] Color[] _Colors = new Color[1] { Color.white };
	[SerializeField] int _TextureSize = 128;
	[SerializeField] FilterMode _FilterMode = FilterMode.Point;

	bool useGradient = false;

	[MenuItem("Tools/Create Palette")]
	public static void ShowWindow() {
		GetWindow(typeof(PaletteCreator));
		GetWindow<PaletteCreator>().Show();
	}

	void OnGUI() {
		EditorGUILayout.Space();

		EditorGUILayout.LabelField("Create Palette:", EditorStyles.boldLabel);
		
		useGradient = EditorGUILayout.Toggle("Use Gradient", useGradient);
		EditorGUILayout.Space();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Texture Name: Assets/", GUILayout.Width(146));
		texName = GUILayout.TextField(texName);
		EditorGUILayout.EndHorizontal();

		if (File.Exists(Application.dataPath + "/" + texName + ".png"))
			EditorGUILayout.HelpBox(texName + ".png already exists and will be replaced.", MessageType.Warning);

		EditorGUILayout.Space();

		SerializedObject sO = new SerializedObject(this);
		SerializedProperty gradient = sO.FindProperty("_Gradient");
		SerializedProperty colorList = sO.FindProperty("_Colors");
		SerializedProperty size = sO.FindProperty("_TextureSize");
		SerializedProperty filterMode = sO.FindProperty("_FilterMode");

		EditorGUI.BeginChangeCheck();

			if (useGradient)
				EditorGUILayout.PropertyField(gradient, true);
			else
				EditorGUILayout.PropertyField(colorList, true);
			EditorGUILayout.Space();

			if (useGradient)
				EditorGUILayout.PropertyField(size, true);
			EditorGUILayout.PropertyField(filterMode, true);

		if (EditorGUI.EndChangeCheck()) {
			sO.ApplyModifiedProperties();
		}
		
		EditorGUILayout.Space();
		if (GUILayout.Button("Create Palette")) {
			CreatePalette();
			Close();
		}
		Repaint();
	}

	void CreatePalette() {
		int size = useGradient ? _TextureSize : _Colors.Length;
		Texture2D texGradient = new Texture2D(size, 1);

		if (useGradient) {
			for (int i = 0; i < _TextureSize; i++) {
				float time = (float)i / _TextureSize;
				Color value = _Gradient.Evaluate(time);
				texGradient.SetPixel(i, 1, value);
			}
		} else {
			for (int i = 0; i < _Colors.Length; i++)
				texGradient.SetPixel(i, 1, _Colors[i]);
		}
		
		texGradient.Apply();

		byte[] bytes = texGradient.EncodeToPNG();
		File.WriteAllBytes(Application.dataPath + "/" + texName + ".png", bytes);
		
		PalettePostProcessor.filterMode = _FilterMode;
		PalettePostProcessor.palettePath = "Assets/" + texName + ".png";
		AssetDatabase.ImportAsset("Assets/" + texName + ".png");
		
		DestroyImmediate(texGradient);
	}
}

class PalettePostProcessor : AssetPostprocessor {
	
	public static string palettePath;
	public static FilterMode filterMode;

	void OnPreprocessTexture() {
		if (assetPath == palettePath) {
			TextureImporter texImport = (TextureImporter)assetImporter;
			texImport.filterMode = filterMode;
			texImport.alphaIsTransparency = true;
			if (filterMode == FilterMode.Point)
				texImport.npotScale = TextureImporterNPOTScale.None;
		}
	}
}