using System.IO;
using System.IO.Compression;
using System.Threading;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace RimuruDev.Unity_CustomBuildUpdater.CustomBuildUpdater.Editor
{
    // TODO: Add set active debug log
    public class BuildVersionUpdater : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        public int callbackOrder => 0;

        private static BuildConfig config;

        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        // ReSharper disable once ConvertToConstant.Local
        private static string defaultBuildPath = "Builds";

        private static void LoadConfig()
        {
            config = Resources.Load<BuildConfig>("Editor/BuildConfig");
            if (config == null)
            {
                // TODO: Add Auto create default config
                Debug.LogError("BuildConfig not found. Please create it in the Resources/Editor folder.");
            }
        }

        public void OnPreprocessBuild(BuildReport report)
        {
            LoadConfig();

            if (config == null)
                return;

            EnsureVersionFileExists();

            var version = GetNextVersion();
            PlayerSettings.bundleVersion = version;

            PlayerSettings.productName = config.productName;
            PlayerSettings.companyName = config.companyName;

            Debug.Log($"New version: {version}");
        }

        public void OnPostprocessBuild(BuildReport report)
        {
            LoadConfig();

            if (config == null)
                return;

            var buildPath = report.summary.outputPath;
            var extension = GetBuildExtension(report.summary.platform);
            var finalBuildPath = GetFinalBuildPath(buildPath, extension);

            if (Directory.Exists(buildPath) || File.Exists(buildPath))
            {
                RenameBuildPath(buildPath, finalBuildPath);
                Debug.Log($"Build renamed to: {finalBuildPath}");
            }

            Thread.Sleep(1000);

            if (config.archiveBuild)
            {
                ArchiveBuild(finalBuildPath);
            }
        }

        private void EnsureVersionFileExists()
        {
            var versionFilePath = Path.Combine(Application.dataPath, "Resources/Editor/version.txt");
            if (!File.Exists(versionFilePath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(versionFilePath));
                File.WriteAllText(versionFilePath, config.initialVersion);
            }
        }

        private string GetNextVersion()
        {
            var versionFilePath = Path.Combine(Application.dataPath, "Resources/Editor/version.txt");
            var version = File.ReadAllText(versionFilePath);
            var versionParts = version.Split('.');
            var major = int.Parse(versionParts[0]);
            var feature = int.Parse(versionParts[1]);
            var bugfix = int.Parse(versionParts[2]);
            var build = int.Parse(versionParts[3]);
            build++;

            switch (config.versionType)
            {
                case VersionType.Major:
                    major++;
                    feature = 0;
                    bugfix = 0;
                    build = 1;
                    break;
                case VersionType.Feature:
                    feature++;
                    bugfix = 0;
                    build = 1;
                    break;
                case VersionType.Bugfix:
                    bugfix++;
                    build = 1;
                    break;
                case VersionType.Build:
                    break;
            }

            version = $"{major}.{feature}.{bugfix}.{build}";
            File.WriteAllText(versionFilePath, version);
            return version;
        }

        private string GetBuildExtension(BuildTarget platform)
        {
            switch (platform)
            {
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return ".exe";
                case BuildTarget.WebGL:
                    return "";
                default:
                    return "";
            }
        }

        private string GetFinalBuildPath(string buildPath, string extension)
        {
            var finalPath = config.buildPathType == BuildPathType.Default
                ? Path.Combine(Application.dataPath, "..", defaultBuildPath, $"{config.companyName}.{config.productName}.v{PlayerSettings.bundleVersion}{extension}")
                : Path.Combine(config.customBuildPath, $"{config.companyName}.{config.productName}.v{PlayerSettings.bundleVersion}{extension}");

            return finalPath;
        }

        private void RenameBuildPath(string oldPath, string newPath)
        {
            var newPathDirectory = Path.GetDirectoryName(newPath);
            if (!Directory.Exists(newPathDirectory))
            {
                Directory.CreateDirectory(newPathDirectory);
            }

            if (Directory.Exists(oldPath))
            {
                Directory.Move(oldPath, newPath);
            }
            else if (File.Exists(oldPath))
            {
                File.Move(oldPath, newPath);
            }
        }

        private void ArchiveBuild(string buildPath)
        {
            var archivePath = buildPath + ".zip";

            if (!File.Exists(archivePath))
            {
                CreateZipFromDirectory(buildPath, archivePath);
            }
            else
            {
                for (var i = 1; i < 100; i++)
                {
                    var numberedArchivePath = buildPath + $"_{i}.zip";
                    if (!File.Exists(numberedArchivePath))
                    {
                        CreateZipFromDirectory(buildPath, numberedArchivePath);
                        break;
                    }
                }
            }

            Debug.Log($"Build archived to: {archivePath}");
        }

        private void CreateZipFromDirectory(string sourceDir, string zipFile)
        {
            using (ZipArchive zip = ZipFile.Open(zipFile, ZipArchiveMode.Create))
            {
                var dirInfo = new DirectoryInfo(sourceDir);

                foreach (FileInfo file in dirInfo.GetFiles())
                {
                    zip.CreateEntryFromFile(file.FullName, file.Name);
                }

                foreach (var dir in dirInfo.GetDirectories())
                {
                    AddDirectoryToZip(zip, dir, dir.Name);
                }
            }
        }

        private void AddDirectoryToZip(ZipArchive zip, DirectoryInfo dir, string entryName)
        {
            foreach (var file in dir.GetFiles())
            {
                zip.CreateEntryFromFile(file.FullName, Path.Combine(entryName, file.Name));
            }

            foreach (var subDir in dir.GetDirectories())
            {
                AddDirectoryToZip(zip, subDir, Path.Combine(entryName, subDir.Name));
            }
        }

        //[MenuItem("RimuruDev Tools/Build Project (DANGEROUS!!!)")]
        public static void BuildProject()
        {
            LoadConfig();

            if (config == null)
                return;

            var buildPath = config.buildPathType == BuildPathType.Default
                ? Path.Combine(Application.dataPath, "..", defaultBuildPath)
                : config.customBuildPath;

            if (!Directory.Exists(buildPath))
            {
                Directory.CreateDirectory(buildPath);
            }

            var selectedPath = EditorUtility.SaveFolderPanel("Choose Build Location", buildPath, "");

            if (!string.IsNullOrEmpty(selectedPath))
            {
                config.customBuildPath = selectedPath;
                EditorUtility.SetDirty(config);

                // Trigger the actual build process (example for WebGL build) :D
                var buildPlayerOptions = new BuildPlayerOptions
                {
                    // TODO: Write a config for the build according to the config.
                    scenes = new[] { "Assets/Scenes/MainScene.unity" },
                    locationPathName = selectedPath,
                    target = BuildTarget.WebGL,
                    options = BuildOptions.None
                };
                BuildPipeline.BuildPlayer(buildPlayerOptions);
            }
        }
    }
}