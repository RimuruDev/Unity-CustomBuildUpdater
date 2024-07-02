using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace RimuruDev.Unity_CustomBuildUpdater.CustomBuildUpdater.Checkers
{
    // TODO: Add log level settings
    public static class InfoYGChecker
    {
        private const string InfoYGPath = "Assets/YandexGame/WorkingData/InfoYG.asset";

        [InitializeOnLoadMethod]
        private static void CheckAndDisableArchivingBuild()
        {
            if (File.Exists(InfoYGPath))
            {
                var infoYGType = GetType("YG.InfoYG");
                if (infoYGType != null)
                {
                    var infoYG = AssetDatabase.LoadAssetAtPath(InfoYGPath, infoYGType) as ScriptableObject;
                    if (infoYG != null)
                    {
                        var archivingBuildField = infoYGType.GetField("archivingBuild");
                        if (archivingBuildField != null)
                        {
                            archivingBuildField.SetValue(infoYG, false);

                            EditorUtility.SetDirty(infoYG);
                            AssetDatabase.SaveAssets();
                            AssetDatabase.Refresh();

                            Debug.Log("InfoYG archivingBuild set to false");
                        }
                        // else Debug.LogWarning("Field archivingBuild not found in InfoYG.");
                    }
                    // else Debug.LogWarning("InfoYG asset exists but could not be loaded.");
                }
                // else Debug.LogWarning("Class InfoYG not found in namespace YG.");
            }
            // else Debug.Log("InfoYG asset does not exist.");
        }

        private static Type GetType(string typeName)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var type = assembly.GetType(typeName);

                if (type != null)
                    return type;
            }

            return null;
        }
    }
}