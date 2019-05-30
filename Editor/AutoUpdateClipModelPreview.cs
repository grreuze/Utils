using System;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using UnityEditor.Animations;

[InitializeOnLoad]
/// <summary>
/// When selecting an animationClip, automatically updates the model in the preview.
/// </summary>
public class AutoUpdateClipModelPreview {

    static AutoUpdateClipModelPreview() {
        Selection.selectionChanged -= OnSelect;
        Selection.selectionChanged += OnSelect;
    }

    static void OnSelect() {
        EditorApplication.delayCall += UpdateModelInPreviewWindow; // delay the call to wait for the avatarPreview to be loaded
    }


    enum PreviewHolder {
        Clip, BlendTree, Transition,
    }


    static void UpdateModelInPreviewWindow() {
        BlendTree blendTree = Selection.activeObject as BlendTree;
        AnimatorTransitionBase trans = Selection.activeObject as AnimatorTransitionBase;
        AnimatorState state = Selection.activeObject as AnimatorState;

        ModelImporter importer = state ? GetModelImporterFromMotion(state.motion)
                               : trans ? GetModelImporterFromTransition(trans)
                               : blendTree ? GetModelImporterFromBlendTreeRecursively(blendTree) 
                               : AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(Selection.activeObject)) as ModelImporter;

        if (importer) {
            GameObject model = (GameObject)AssetDatabase.LoadAssetAtPath(AssetDatabase.GetAssetPath(importer.sourceAvatar), typeof(GameObject));
            if (model) {
                object _preview = GetAvatarPreview(blendTree ? PreviewHolder.BlendTree : trans ? PreviewHolder.Transition : PreviewHolder.Clip);
                if (_preview != null) {
                    Type avatarPreview = Type.GetType("UnityEditor.AvatarPreview,UnityEditor");
                    avatarPreview.GetMethod("SetPreview", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(_preview, new object[] { model });

                    EditorWindow[] allWindows = Resources.FindObjectsOfTypeAll<EditorWindow>();
                    foreach (EditorWindow window in allWindows) {
                        if (window.GetType().Name.Contains("Preview")) {
                            window.Repaint();
                            return;
                        } else if (window.GetType().Name.Contains("Inspector"))
                            window.Repaint();
                    }

                }
            }
        }
    }

    static object GetAvatarPreview(PreviewHolder holder) {

        switch (holder) {
            case PreviewHolder.Clip: {
                Type clipEditor = GetEditorType("AnimationClipEditor");
                UnityEngine.Object[] openEditors = Resources.FindObjectsOfTypeAll(clipEditor);

                if (openEditors.Length > 0)
                    return GetPrivateVariable("m_AvatarPreview", openEditors[0], clipEditor);
                break;
            }

            case PreviewHolder.BlendTree: {
                Type blendTreeInspector = GetEditorType("BlendTreeInspector");
                UnityEngine.Object[] openInspectors = Resources.FindObjectsOfTypeAll(blendTreeInspector);

                if (openInspectors.Length > 0) {
                    Type previewBlendTree = GetEditorType("PreviewBlendTree");
                    var _previewHold = GetPrivateVariable("m_PreviewBlendTree", openInspectors[0], blendTreeInspector);

                    return GetPrivateVariable("m_AvatarPreview", _previewHold, previewBlendTree);
                }
                break;
            }

            case PreviewHolder.Transition: {
                Type transEditor = Type.GetType("UnityEditor.Graphs.AnimationStateMachine.AnimatorStateTransitionInspector,UnityEditor.Graphs");
                UnityEngine.Object[] openInspectors = Resources.FindObjectsOfTypeAll(transEditor);

                if (openInspectors.Length > 0) {
                    Type transPreview = GetEditorType("TransitionPreview");
                    var _previewHold = GetPrivateVariable("m_TransitionPreview", openInspectors[0], transEditor);

                    return GetPrivateVariable("m_AvatarPreview", _previewHold, transPreview);
                }
                break;
            }
        }
        return null;
    }

    static ModelImporter GetModelImporterFromTransition(AnimatorTransitionBase transition) {
        if (!transition) return null;

        if (transition.destinationState) {
            return GetModelImporterFromMotion(transition.destinationState.motion);

        } else {
            Type transEditor = Type.GetType("UnityEditor.Graphs.AnimationStateMachine.AnimatorStateTransitionInspector,UnityEditor.Graphs");
            UnityEngine.Object[] openInspectors = Resources.FindObjectsOfTypeAll(transEditor);
            if (openInspectors.Length > 0) {
                AnimatorState source = transEditor.GetMethod("GetSourceState", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(openInspectors[0], new object[] { 0 }) as AnimatorState;

                if (source)
                    return GetModelImporterFromMotion(source.motion);
                else return null;

            } else {
                return null;
            }
        }
    }

    static ModelImporter GetModelImporterFromBlendTreeRecursively(BlendTree blendTree) {
        if (!blendTree || blendTree.children == null || blendTree.children.Length == 0) return null;

        return GetModelImporterFromMotion(blendTree.children[0].motion);
    }

    static ModelImporter GetModelImporterFromMotion(Motion motion) {
        if (motion is BlendTree)
            return GetModelImporterFromBlendTreeRecursively(motion as BlendTree);
        if (motion is AnimationClip)
            return AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(motion)) as ModelImporter;
        return null;
    }


    static object GetPrivateVariable(string name, object instance, Type instanceType) {
        return instanceType.GetField(name, BindingFlags.NonPublic | BindingFlags.Instance).GetValue(instance);
    }

    static Type GetEditorType(string name) {
        return Type.GetType(string.Format("UnityEditor.{0},UnityEditor", name));
    }

}
