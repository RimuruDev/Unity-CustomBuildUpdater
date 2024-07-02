using UnityEditor;
using UnityEngine;

namespace RimuruDev.Unity_CustomBuildUpdater.CustomBuildUpdater.Editor
{
    [CustomEditor(typeof(BuildConfig))]
    public class BuildConfigEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var config = (BuildConfig)target;

            config.companyName = EditorGUILayout.TextField("Company Name", config.companyName);
            config.productName = EditorGUILayout.TextField("Product Name", config.productName);
            config.buildPathType = (BuildPathType)EditorGUILayout.EnumPopup("Build Path Type", config.buildPathType);

            if (config.buildPathType == BuildPathType.Custom)
            {
                config.customBuildPath = EditorGUILayout.TextField("Custom Build Path", config.customBuildPath);
            }

            config.archiveBuild = EditorGUILayout.Toggle("Archive Build", config.archiveBuild);
            config.versionType = (VersionType)EditorGUILayout.EnumPopup("Version Type", config.versionType);
            config.versionPattern = EditorGUILayout.TextField("Version Pattern", config.versionPattern);

            if (GUILayout.Button("Initialize BuildConfig"))
            {
                CreateOrSelectBuildConfig();
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(config);
            }
        }

        [MenuItem("RimuruDev Tools/Initialize BuildConfig")]
        private static void CreateOrSelectBuildConfig()
        {
            var path = "Assets/Resources/Editor/BuildConfig.asset";
            var config = AssetDatabase.LoadAssetAtPath<BuildConfig>(path);

            if (config == null)
            {
                config = CreateInstance<BuildConfig>();
                config.companyName = PlayerSettings.companyName;
                config.productName = PlayerSettings.productName;

                if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                {
                    AssetDatabase.CreateFolder("Assets", "Resources");
                }

                if (!AssetDatabase.IsValidFolder("Assets/Resources/Editor"))
                {
                    AssetDatabase.CreateFolder("Assets/Resources", "Editor");
                }

                AssetDatabase.CreateAsset(config, path);
                AssetDatabase.SaveAssets();
                Debug.Log("BuildConfig created at " + path);
            }
            else
            {
                Debug.Log("BuildConfig already exists at " + path);
            }

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = config;
        }
    }
}