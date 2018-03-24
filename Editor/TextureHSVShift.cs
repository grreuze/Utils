using UnityEngine;
using UnityEditor;
using System.IO;

public class TextureHSVShift : EditorWindow
{

    string texName = "Graph/Textures/HSVShift";

    [SerializeField, Range(-1, 1)] float hueShift = 0;
	[SerializeField, Range(-1, 1)] float saturationShift = 0;
	[SerializeField, Range(-1, 1)] float valueShift = 0;

	[SerializeField] Texture2D shiftedTexture;

    [MenuItem("Tools/Texture HSV Shift %U")]
    public static void ShowWindow()
    {
        GetWindow(typeof(TextureHSVShift));
        GetWindow<TextureHSVShift>().Show();
    }

    void OnGUI()
    {
        Texture[] textures = Selection.GetFiltered<Texture>(SelectionMode.Assets);

        if (textures.Length == 0)
        {
            EditorGUILayout.HelpBox("Select a Texture in order to change its hue.", MessageType.Warning);
            return;
        }
        EditorGUILayout.Space();

        Texture texture = textures[0];

		EditorGUILayout.LabelField("Hue Shift Texture:", EditorStyles.boldLabel);
        EditorGUI.DrawPreviewTexture(new Rect(130, 5, 20, 20), texture);
        
        EditorGUILayout.Space();
        
        texName = AssetDatabase.GetAssetPath(texture) + "_HSV";
        texName = GUILayout.TextField(texName);

        if (File.Exists(Application.dataPath + "/" + texName + ".png"))
            EditorGUILayout.HelpBox(texName + ".png already exists and will be replaced.", MessageType.Warning);

        EditorGUILayout.Space();

        SerializedObject sO = new SerializedObject(this);
        SerializedProperty _hueShift = sO.FindProperty("hueShift");
		SerializedProperty _saturationShift = sO.FindProperty("saturationShift");
		SerializedProperty _valueShift = sO.FindProperty("valueShift");

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(_hueShift);
		EditorGUILayout.PropertyField(_saturationShift);
		EditorGUILayout.PropertyField(_valueShift);

		if (!shiftedTexture)
			shiftedTexture = texture as Texture2D;

		EditorGUI.DrawPreviewTexture(new Rect(20, 150, 100, 100), shiftedTexture);


		if (EditorGUI.EndChangeCheck()) {
			sO.ApplyModifiedProperties();
		}

        EditorGUILayout.Space();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.Space();
		if (GUILayout.Button("Create Texture Copy")) {
			shiftedTexture = ShiftTexture(texture, hueShift, saturationShift, valueShift);
			CreateTextureCopy(shiftedTexture);
			Close();
		}
		EditorGUILayout.Space();
		EditorGUILayout.EndHorizontal();

		for (int i = 0; i < 21; i++)
			EditorGUILayout.Space();

		if (GUILayout.Button("Preview Texture", GUILayout.Width(140)))
			shiftedTexture = ShiftTexture(texture, hueShift, saturationShift, valueShift);
		
        Repaint();
    }

	Texture2D ShiftTexture(Texture texture, float hueShift, float saturationShift, float valueShift) {

		Texture2D srcTexture = texture as Texture2D;
		Texture2D newTexture = new Texture2D(texture.width, texture.height);

		Color[] src = srcTexture.GetPixels();
		Color[] pix = newTexture.GetPixels();

		for (int i = 0; i < texture.height; i++) {
			for (int j = 0; j < texture.width; j++) {
				float h = 0, s = 0, v = 0;
				Color.RGBToHSV(src[(i * texture.width) + j], out h, out s, out v);

				h += hueShift;
				if (h > 1)
					h -= 1;
				else if (h < 0)
					h += 1;

				s = Mathf.Clamp01(s + saturationShift);

				v = Mathf.Clamp01(v + valueShift);
				
				pix[i] = Color.HSVToRGB(h, s, v);
				newTexture.SetPixel(j, i, pix[i]);
			}
		}

		newTexture.Apply();
		return newTexture;
	}
	
	void CreateTextureCopy(Texture2D newTexture) {
		
        byte[] bytes = newTexture.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath.Remove(Application.dataPath.Length - 7) + "/" + texName + ".png", bytes);
        
        HueShiftPostProcessor.texturePath = texName + ".png";
        AssetDatabase.ImportAsset(texName + ".png");

        DestroyImmediate(newTexture);
    }
}

class HueShiftPostProcessor : AssetPostprocessor
{

    public static string texturePath;
   /* public static FilterMode filterMode;

    void OnPreprocessTexture()
    {
        if (assetPath == texturePath)
        {

        }
    }*/
}