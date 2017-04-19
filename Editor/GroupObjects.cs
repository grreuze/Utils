using UnityEngine;
using UnityEditor;

public class GroupObjects : EditorWindow {

    string groupName = "Group";
    bool centerGroup = true, starting = true;
    Vector3 groupPosition = Vector3.zero;

    [MenuItem("GameObject/Group Objects %G", false, -10)]
    public static void ShowWindow() {
        GetWindow(typeof(GroupObjects));
        GetWindow<GroupObjects>().Show();
    }

    void OnGUI() {
        Event e = Event.current;
        EditorGUILayout.LabelField("Chose Parent name:", EditorStyles.boldLabel);
        GUI.SetNextControlName("TextField");
        groupName = GUILayout.TextField(groupName, GUILayout.Width(200));
        if (starting) {
            EditorGUI.FocusTextInControl("TextField");
            starting = false;
        }
        centerGroup = GUILayout.Toggle(centerGroup, "Center Parent Object");
        groupPosition = centerGroup ? Vector3.zero : EditorGUILayout.Vector3Field("Parent Object Position", groupPosition);
        if (GUILayout.Button("Create Group") || e.isKey && e.keyCode == KeyCode.Return) {
            Group();
            Close();
        }
        Repaint();
    }

    void Group() {
        GameObject newGroup = new GameObject(groupName);
        Undo.RegisterCreatedObjectUndo(newGroup, "New Group: " + newGroup.name);

        if (Selection.gameObjects.Length > 0) {
            Transform commonParent = Selection.gameObjects[0].transform.parent;

            foreach (GameObject go in Selection.gameObjects) {
                if (centerGroup) groupPosition += go.transform.position;
                if (go.transform.parent != commonParent) commonParent = null;
            }
            newGroup.transform.parent = commonParent;
            if (centerGroup) {
                groupPosition /= (Selection.gameObjects.Length);
                if (commonParent)
                    groupPosition = commonParent.InverseTransformPoint(groupPosition);
            }
            newGroup.transform.localPosition = groupPosition;
            foreach (GameObject go in Selection.gameObjects)
                Undo.SetTransformParent(go.transform, newGroup.transform, "GroupObjects");
        }
        Selection.activeGameObject = newGroup;
    }
}
