using System;
using UnityEngine;

namespace RimuruDev.Unity_CustomBuildUpdater.CustomBuildUpdater.Editor
{
    [CreateAssetMenu(fileName = "BuildConfig", menuName = "Configs/Build/BuildConfig", order = 1)]
    public class BuildConfig : ScriptableObject
    {
        public string companyName = "AbyssMoth";
        public string productName = "SuperGame";
        public string initialVersion = "1.0.0.0";
        public BuildPathType buildPathType = BuildPathType.Default;
        public string customBuildPath = "Builds";
        public bool archiveBuild = true;
        public VersionType versionType = VersionType.Build;
        public string versionPattern = "com.{company}.{product}.v{version}";
    }
}