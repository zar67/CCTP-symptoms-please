using SymptomsPlease.Debugging.Logging;
using System;
using UnityEditor;
using UnityEngine;

public class LoggingSettingsEditorWindow : EditorWindow
{
    [MenuItem("SymptomsPlease/Windows/Logging Settings Editor")]
    public static void ShowEditorWindow()
    {
        GetWindow(typeof(LoggingSettingsEditorWindow), false, "Logging Settings Editor");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Enable All"))
        {
            EnableAllChannels(true);
        }

        if (GUILayout.Button("Disable All"))
        {
            EnableAllChannels(false);
        }

        SymptomsPlease.Debugging.Logging.CustomLogger.GetSettingsFileData();
        foreach (string value in Enum.GetNames(typeof(LoggingChannels)))
        {
            var channel = (LoggingChannels)Enum.Parse(typeof(LoggingChannels), value);

            bool channelEnabled = EditorGUILayout.ToggleLeft(value, SymptomsPlease.Debugging.Logging.CustomLogger.IsChannelEnabled(channel));
            if (channelEnabled)
            {
                SymptomsPlease.Debugging.Logging.CustomLogger.EnableChannel(channel);
            }
            else
            {
                SymptomsPlease.Debugging.Logging.CustomLogger.DisableChannel(channel);
            }
        }

        SymptomsPlease.Debugging.Logging.CustomLogger.UpdateSettingsFile();
    }

    private void EnableAllChannels(bool value)
    {
        foreach (string channelString in Enum.GetNames(typeof(LoggingChannels)))
        {
            var channel = (LoggingChannels)Enum.Parse(typeof(LoggingChannels), channelString);
            if (value)
            {
                SymptomsPlease.Debugging.Logging.CustomLogger.EnableChannel(channel);
            }
            else
            {
                SymptomsPlease.Debugging.Logging.CustomLogger.DisableChannel(channel);
            }
        }
    }
}
