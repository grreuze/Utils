using UnityEngine;
using UnityEditor;

public class GroupObjects : EditorWindow {

    string GroupName = "Group";

    [MenuItem("GameObject/Group Objects %G", false, -10)]
    public static void ShowWindow() {
        GetWindow(typeof(GroupObjects));
        GetWindow<GroupObjects>().Show();
    }

    void OnGUI() {
        Event e = Event.current;
        EditorGUILayout.LabelField("Chose Parent name:", EditorStyles.boldLabel);
        GUI.SetNextControlName("TextField");
        GroupName = GUILayout.TextField(GroupName, GUILayout.Width(200));
        EditorGUI.FocusTextInControl("TextField");
        if (GUILayout.Button("Create Group") || e.isKey && e.keyCode == KeyCode.Return) {
            Group();
            Close();
        }
        Repaint();
    }

    void Group() {
        GameObject newGroup = new GameObject(GroupName);

        Undo.RegisterCreatedObjectUndo(newGroup, "New Group: " + newGroup.name);
        Vector3 pos = Vector3.zero;
        
        foreach (GameObject go in Selection.gameObjects)
            pos += go.transform.localPosition;

        if (Selection.gameObjects.Length > 0) {
            newGroup.transform.position = pos / (Selection.gameObjects.Length);
            foreach (GameObject go in Selection.gameObjects)
                Undo.SetTransformParent(go.transform, newGroup.transform, "GroupObjects");
        }
        Selection.activeGameObject = newGroup;
    }
}
