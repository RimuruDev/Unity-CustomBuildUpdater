using System.IO;
using UnityEditor;
using UnityEngine;

namespace RimuruDev.Unity_CustomBuildUpdater.CustomBuildUpdater.Editor
{
    [CustomEditor(typeof(BuildConfig))]
    public class BuildConfigEditor : UnityEditor.Editor
    {
        private const string PathToConfig = "Assets/Resources/Editor/BuildConfig.asset";

        public override void OnInspectorGUI()
        {
            var config = (BuildConfig)target;

            config.companyName = EditorGUILayout.TextField("Company Name", config.companyName);
            config.productName = EditorGUILayout.TextField("Product Name", config.productName);
            config.initialVersion = EditorGUILayout.TextField("Initial Version", config.initialVersion);
            config.buildPathType = (BuildPathType)EditorGUILayout.EnumPopup("Build Path Type", config.buildPathType);

            if (config.buildPathType == BuildPathType.Custom)
            {
                config.customBuildPath = EditorGUILayout.TextField("Custom Build Path", config.customBuildPath);
            }

            config.archiveBuild = EditorGUILayout.Toggle("Archive Build", config.archiveBuild);
            config.versionType = (VersionType)EditorGUILayout.EnumPopup("Version Type", config.versionType);
            config.versionPattern = EditorGUILayout.TextField("Version Pattern", config.versionPattern);

            // if (GUILayout.Button("Initialize BuildConfig"))
            // {
            //     CreateOrSelectBuildConfig();
            // }

            if (GUILayout.Button("Update Current Version"))
            {
                UpdateCurrentVersion();
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(config);
                AssetDatabase.SaveAssets();
            }
        }

        [MenuItem("RimuruDev Tools/Initialize BuildConfig")]
        private static void CreateOrSelectBuildConfig()
        {
            var config = GetConfig();

            if (config == null)
            {
                config = CreateInstance<BuildConfig>();
                config.companyName = PlayerSettings.companyName;
                config.productName = PlayerSettings.productName;
                config.initialVersion = "1.0.0.0";
                config.buildPathType = BuildPathType.Default;
                config.archiveBuild = true;
                config.versionType = VersionType.Build;
                config.versionPattern = "com.{company}.{product}.v{version}";

                if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                {
                    AssetDatabase.CreateFolder("Assets", "Resources");
                }

                if (!AssetDatabase.IsValidFolder("Assets/Resources/Editor"))
                {
                    AssetDatabase.CreateFolder("Assets/Resources", "Editor");
                }

                AssetDatabase.CreateAsset(config, PathToConfig);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                Debug.Log("BuildConfig created at " + PathToConfig);
            }
            else
            {
                Debug.Log("BuildConfig already exists at " + PathToConfig);
            }

            var versionFilePath = Path.Combine(Application.dataPath, "Resources/Editor/version.txt");
            if (!File.Exists(versionFilePath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(versionFilePath));
                File.WriteAllText(versionFilePath, config.initialVersion);
            }

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = config;
        }

        private void UpdateCurrentVersion()
        {
            var config = GetConfig();

            var versionFilePath = Path.Combine(Application.dataPath, "Resources/Editor/version.txt");
            if (!File.Exists(versionFilePath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(versionFilePath));
                File.WriteAllText(versionFilePath, config.initialVersion);
            }
            else
            {
                File.WriteAllText(versionFilePath, config.initialVersion);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static BuildConfig GetConfig() => AssetDatabase.LoadAssetAtPath<BuildConfig>(PathToConfig);
    }
}