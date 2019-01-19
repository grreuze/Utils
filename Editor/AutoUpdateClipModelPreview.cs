using System;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using UnityEditor.Animations;

namespace jumpship {
    /// <summary>
    /// When selecting an animationClip, automatically updates the model in the preview.
    /// </summary>
    public class AutoUpdateClipModelPreview : Editor {

        private void OnEnable() {
            Selection.selectionChanged -= OnSelect;
            if (Selection.selectionChanged != null) {
                foreach (Delegate m in Selection.selectionChanged.GetInvocationList()) {
                    if (m.Method.DeclaringType.FullName == GetType().FullName) {
                        Selection.selectionChanged -= (Action)m;
                    }
                }
            }
            Selection.selectionChanged += OnSelect;
        }

        private void OnDestroy() {
            Selection.selectionChanged -= OnSelect;
        }

        void OnSelect() {
            EditorApplication.delayCall += UpdateModelInPreviewWindow; // delay the call to wait for the avatarPreview to be loaded
        }


        void UpdateModelInPreviewWindow() {
            BlendTree blendTree = Selection.activeObject as BlendTree;
            ModelImporter importer = blendTree ? GetModelImporterFromBlendTreeRecursively(blendTree) 
                                   : AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(Selection.activeObject)) as ModelImporter;
            if (importer) {
                GameObject model = (GameObject)AssetDatabase.LoadAssetAtPath(AssetDatabase.GetAssetPath(importer.sourceAvatar), typeof(GameObject));
                if (model) {
                    object _preview = GetAvatarPreview(blendTree);
                    if (_preview != null) {
                        Type avatarPreview = Type.GetType("UnityEditor.AvatarPreview,UnityEditor");
                        avatarPreview.GetMethod("SetPreview", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(_preview, new object[] { model });
                    }
                }
            }
        }
        
        object GetAvatarPreview(bool isBlendTree) {

            if (isBlendTree) {
                Type blendTreeInspector = GetEditorType("BlendTreeInspector");
                UnityEngine.Object[] openInspectors = Resources.FindObjectsOfTypeAll(blendTreeInspector);

                if (openInspectors.Length > 0) {
                    Type previewBlendTree = GetEditorType("PreviewBlendTree");
                    var _previewHold = GetPrivateVariable("m_PreviewBlendTree", openInspectors[0], blendTreeInspector);

                    return GetPrivateVariable("m_AvatarPreview", _previewHold, previewBlendTree);
                }
            }
            else {
                Type clipEditor = GetEditorType("AnimationClipEditor");
                UnityEngine.Object[] openEditors = Resources.FindObjectsOfTypeAll(clipEditor);

                if (openEditors.Length > 0)
                    return GetPrivateVariable("m_AvatarPreview", openEditors[0], clipEditor);
            }
            return null;
        }


        ModelImporter GetModelImporterFromBlendTreeRecursively(BlendTree blendTree) {
            if (!blendTree || blendTree.children == null || blendTree.children.Length == 0) return null;

            Motion motion = blendTree.children[0].motion; // get the model from the first child

            if (motion is BlendTree)
                return GetModelImporterFromBlendTreeRecursively(motion as BlendTree);
            if (motion is AnimationClip)
                return AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(motion)) as ModelImporter;
            return null;
        }


        object GetPrivateVariable(string name, object instance, Type instanceType) {
            return instanceType.GetField(name, BindingFlags.NonPublic | BindingFlags.Instance).GetValue(instance);
        }

        Type GetEditorType(string name) {
            return Type.GetType(string.Format("UnityEditor.{0},UnityEditor", name));
        }

    }
}