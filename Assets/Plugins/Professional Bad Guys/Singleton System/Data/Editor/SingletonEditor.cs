
#if ODIN_INSPECTOR
using Sirenix.OdinInspector.Editor;
#endif

using UnityEditor;
using UnityEngine;

namespace SingletonSystem {

    [CustomEditor(typeof(SingletonBase), true, isFallback = true)]
#if ODIN_INSPECTOR
    public class SingletonEditor : OdinEditor {
#else
    public class SingletonEditor : Editor {
#endif

        public override void OnInspectorGUI() {

            base.OnInspectorGUI();

            var path = AssetDatabase.GetAssetPath(target);

            if (!PathUtilities.IsPathWithinResources(path)) {
                string message = $"This Singleton Asset must be within 'Resources'.";
                EditorGUILayout.HelpBox(message, MessageType.Warning, true);
            }

            GUILayout.Space(20);

            if (GUILayout.Button("Select within Project Settings")) {
                var settingsPath = (target as SingletonBase).GetProjectSettingsPath();
                SettingsService.OpenProjectSettings(settingsPath);
            }

            if (GUILayout.Button("Select asset object in Resources")) {
                Selection.activeObject = target;
                EditorGUIUtility.PingObject(Selection.activeObject);
            }
        }
    }
}