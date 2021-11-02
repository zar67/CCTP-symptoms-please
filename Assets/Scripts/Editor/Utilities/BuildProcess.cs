using SymptomsPlease.Debugging.Logging;
using SymptomsPlease.SceneManagement;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace SymptomsPlease.Utilities.Processes
{
    internal class BuildProcess : IPreprocessBuildWithReport
    {
        #region EditorPrefVariables

        private const string EDITORPREFS_SCENE_DATA_PATH = "SymptomsPlease.BuildProcess.BuildSceneDataPath";
        private const string EDITORPREFS_BUILD_LOCATION_PATH = "SymptomsPlease.BuildProcess.BuildLocationPath";

        public static string SceneDataPath
        {
            get => EditorPrefs.GetString(EDITORPREFS_SCENE_DATA_PATH, "null");
            set => EditorPrefs.SetString(EDITORPREFS_SCENE_DATA_PATH, value);
        }

        public static string BuildNumber => DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");

        public static string BuildLocationPath
        {
            get => EditorPrefs.GetString(EDITORPREFS_BUILD_LOCATION_PATH, "null");
            set => EditorPrefs.SetString(EDITORPREFS_BUILD_LOCATION_PATH, value);
        }

        public int callbackOrder => 0;

        [MenuItem("SymptomsPlease/Build/Settings/Select Scene Data...")]
        private static void SelectSceneData()
        {
            string sceneDataPath = EditorUtility.OpenFilePanel("Select Scene Data", Application.dataPath, "asset");
            sceneDataPath = sceneDataPath.Replace(Application.dataPath, "Assets");

            if (!string.IsNullOrEmpty(sceneDataPath))
            {
                SceneDataPath = sceneDataPath;
            }
        }

        [MenuItem("SymptomsPlease/Build/Settings/Select Build Location...")]
        private static void SelectBuildLocation()
        {
            string buildLocationPath = EditorUtility.OpenFolderPanel("Select Build Location", Application.dataPath, "");
            buildLocationPath = buildLocationPath.Replace(Application.dataPath, "Assets");

            if (!string.IsNullOrEmpty(buildLocationPath))
            {
                BuildLocationPath = buildLocationPath;
            }
        }

        #endregion

        #region LocalBuilds

        [MenuItem("SymptomsPlease/Build/Local Build/Release/iOS")]
        public static void BuildIOSRelease()
        {
            BuildPlayerOptions options = GenericBuildInitialise(BuildTarget.iOS, true);
            GenericBuild(options);
        }

        [MenuItem("SymptomsPlease/Build/Local Build/Development/iOS")]
        public static void BuildIOSDevelopment()
        {
            BuildPlayerOptions options = GenericBuildInitialise(BuildTarget.iOS, false);
            GenericBuild(options);
        }

        [MenuItem("SymptomsPlease/Build/Local Build/Release/Android")]
        public static void BuildAndroidRelease()
        {
            BuildPlayerOptions options = GenericBuildInitialise(BuildTarget.Android, true);
            GenericBuild(options);
        }

        [MenuItem("SymptomsPlease/Build/Local Build/Development/Android")]
        public static void BuildAndroidDevelopment()
        {
            BuildPlayerOptions options = GenericBuildInitialise(BuildTarget.Android, false);
            GenericBuild(options);
        }

        [MenuItem("SymptomsPlease/Build/Local Build/Release/Standalone Windows")]
        public static void BuildsWindowsRelease()
        {
            BuildPlayerOptions options = GenericBuildInitialise(BuildTarget.StandaloneWindows, true);
            GenericBuild(options);
        }

        [MenuItem("SymptomsPlease/Build/Local Build/Development/Standalone Windows")]
        public static void BuildWindowsDevelopment()
        {
            BuildPlayerOptions options = GenericBuildInitialise(BuildTarget.StandaloneWindows, false);
            GenericBuild(options);
        }

        #endregion

        public void OnPreprocessBuild(BuildReport report)
        {
            SceneData data = GetBuildSceneData();
            if (data == null)
            {
                throw new System.OperationCanceledException($"[{LoggingChannels.CustomBuildProcess}] - Build Scene Data not found at: {SceneDataPath}");
            }

            data.BuildSceneDataListToSettings();
        }

        public static BuildPlayerOptions GenericBuildInitialise(BuildTarget buildTarget, bool release)
        {
            var buildOptions = new BuildPlayerOptions();
            var defines = new List<string>();

            SceneData data = GetBuildSceneData();
            buildOptions.scenes = data.GetIncludedScenePaths().ToArray();

            buildOptions.target = buildTarget;
            BuildTargetGroup buildTargetGroup = GetBuildTargetGroup(buildTarget);
            buildOptions.targetGroup = buildTargetGroup;

            string subDirectory = "";
            if (buildTargetGroup == BuildTargetGroup.Standalone)
            {
                subDirectory = $"{PlayerSettings.productName}_Build-{BuildNumber}";
            }

            string buildPath = Path.Combine(BuildLocationPath, $"{buildTarget}", subDirectory);
            if (!Directory.Exists(buildPath))
            {
                Directory.CreateDirectory(buildPath);
            }

            string extension = GetExtension(buildTarget);

            if (release)
            {
                buildOptions.locationPathName = $"{buildPath}/{PlayerSettings.productName}_Build-{BuildNumber}_Release{extension}";
                defines.Add("VERSION_RELEASE");
            }
            else
            {
                buildOptions.locationPathName = $"{buildPath}/{PlayerSettings.productName}_Build-{BuildNumber}_Development{extension}";
                buildOptions.options |= BuildOptions.Development;
                defines.Add("VERSION_DEVELOPMENT");
            }

            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildOptions.targetGroup, string.Join(";", defines));
            return buildOptions;
        }

        public static SceneData GetBuildSceneData()
        {
            try
            {
                var data = AssetDatabase.LoadAssetAtPath(SceneDataPath, typeof(SceneData)) as SceneData;
                return data;
            }
            catch
            {
                return null;
                throw new System.OperationCanceledException($"[{LoggingChannels.CustomBuildProcess}] - Build Scene Data not found at: {SceneDataPath}");
            }
        }

        private static void GenericBuild(BuildPlayerOptions options)
        {
            BuildReport report = BuildPipeline.BuildPlayer(options);
            if (report.summary.result == BuildResult.Succeeded)
            {
                CustomLogger.Debug(LoggingChannels.CustomBuildProcess, "Build Suceeded! :)");
            }
            else if (report.summary.result == BuildResult.Failed)
            {
                CustomLogger.Error(LoggingChannels.CustomBuildProcess, "Build Failed! :(");
            }
        }

        private static BuildTargetGroup GetBuildTargetGroup(BuildTarget target)
        {
            switch (target)
            {
                case BuildTarget.Android:
                {
                    return BuildTargetGroup.Android;
                }
                case BuildTarget.iOS:
                {
                    return BuildTargetGroup.iOS;
                }
                case BuildTarget.WebGL:
                {
                    return BuildTargetGroup.WebGL;
                }
                case BuildTarget.StandaloneOSX:
                case BuildTarget.StandaloneWindows:
                {
                    return BuildTargetGroup.Standalone;
                }
            }

            return BuildTargetGroup.Unknown;
        }

        private static string GetExtension(BuildTarget target)
        {
            switch (target)
            {
                case BuildTarget.Android:
                {
                    return ".apk";
                }
                case BuildTarget.iOS:
                case BuildTarget.WebGL:
                {
                    return "";
                }
                case BuildTarget.StandaloneOSX:
                {
                    return ".app";
                }
                case BuildTarget.StandaloneWindows:
                {
                    return ".exe";
                }
            }

            return "";
        }
    }
}