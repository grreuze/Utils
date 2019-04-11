using System;
using UnityEngine;
using UnityEditor;
using System.Reflection;

public class ConsoleScrollUtility : EditorWindow {

    [MenuItem("Window/Console Utility", priority = 2200)]
    public static void ShowWindow() {
        GetWindow(typeof(ConsoleScrollUtility), false, "Console Utility");
    }

    bool initialized;

    bool lockToTop;
    bool autoScroll;

    Type ConsoleWindow;
    Type ListViewState;
    object console;
    object listView;

    //

    bool Initialize() {

        ConsoleWindow = GetEditorType("ConsoleWindow");
        ListViewState = GetEditorType("ListViewState");

        UnityEngine.Object[] openWindows = Resources.FindObjectsOfTypeAll(ConsoleWindow);

        if (openWindows.Length > 0) {

            console = openWindows[0];
            listView = GetPrivateVariable("m_ListView", openWindows[0], ConsoleWindow);
            return initialized = true;

        } else return false;
    }


    private void OnGUI() {

        // initialize

        if (!Initialize()) {
            EditorGUILayout.HelpBox("Console Window is not opened!", MessageType.Error);
            return;
        }


        // draw buttons

        float height = (Screen.height - 28) / 2;

        if (Screen.width > Screen.height) {
            height = Screen.height - 28;
            EditorGUILayout.BeginHorizontal();
        }

        EditorGUI.BeginChangeCheck();
        lockToTop = GUILayout.Toggle(lockToTop, "Lock to\ntop", "Button", GUILayout.Height(height));
        if (EditorGUI.EndChangeCheck() && lockToTop & autoScroll) {
            autoScroll = false;
        }

        EditorGUI.BeginChangeCheck();
        autoScroll = GUILayout.Toggle(autoScroll, "Lock to\nbottom", "Button", GUILayout.Height(height));
        if (EditorGUI.EndChangeCheck() && lockToTop & autoScroll) {
            lockToTop = false;
        }

        if (Screen.width > Screen.height)
            EditorGUILayout.EndHorizontal();


        // do the thing

        if (console != null && (autoScroll | lockToTop)) {
            MoveScrollPos();
        }
    }


    void MoveScrollPos() {
        Vector2 scrollValue = (Vector2)GetPrivateVariable("scrollPos", listView, ListViewState);

        scrollValue.y = lockToTop ? 0 : float.PositiveInfinity;

        ListViewState.GetField("scrollPos", BindingFlags.Public | BindingFlags.Instance).SetValue(listView, scrollValue);

        ConsoleWindow.GetMethod("Repaint").Invoke(console, null);
    }


    object GetPrivateVariable(string name, object instance, Type instanceType) {
        return instanceType.GetField(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(instance);
    }

    Type GetEditorType(string name) {
        return Type.GetType(string.Format("UnityEditor.{0},UnityEditor", name));
    }

}
