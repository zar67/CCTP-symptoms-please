using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SymptomsPlease.Debugging.Logging
{
    /// <summary>
    /// Static class to handle custom logging to the UnityConsole.
    /// </summary>
    public static class CustomLogger
    {
        /// <summary>
        /// Struct to hold the data needed to save the status of the RowbotLogger.
        /// </summary>
        [Serializable]
        public struct SaveData
        {
            /// <summary>
            /// List of the currently enabled channels.
            /// </summary>
            public List<LoggingChannels> EnabledChannels;
        }

        /// <summary>
        /// Const path string to the settings file directory.
        /// </summary>
        public const string LOGGING_SETTINGS_FILE_PATH = "debug/";

        /// <summary>
        /// Const string name of the logging settings file.
        /// </summary>
        public const string LOGGING_SETTINGS_FILE_NAME = "LoggingSettings.sav";

        private static List<LoggingChannels> m_enabledChannels = null;

        /// <summary>
        /// Enables a logging channel so the logs will show in the console.
        /// </summary>
        /// <param name="channel">The channel to enable.</param>
        public static void EnableChannel(LoggingChannels channel)
        {
            GetSettingsFileData();

            if (!m_enabledChannels.Contains(channel))
            {
                m_enabledChannels.Add(channel);
                UpdateSettingsFile();
            }
        }

        /// <summary>
        /// Checks if a channel is enabled and will show in the console or not.
        /// </summary>
        /// <param name="channel">The identifier of the channel to check.</param>
        /// <returns>
        /// <para> true: channel is enabled.</para>
        /// <para> false: channel is disabled.</para>
        /// </returns>
        public static bool IsChannelEnabled(LoggingChannels channel)
        {
            return m_enabledChannels.Contains(channel);
        }

        /// <summary>
        /// Disables a channel, the logs will no longer appear in the console.
        /// </summary>
        /// <param name="channel">The identifier of the channel to disable.</param>
        public static void DisableChannel(LoggingChannels channel)
        {
            GetSettingsFileData();

            m_enabledChannels.Remove(channel);
            UpdateSettingsFile();
        }

        /// <summary>
        /// Sends a log to the console with the severity of a Debug.
        /// </summary>
        /// <param name="channel">The type of channel to send the message via.</param>
        /// <param name="message">String value of the message to send.</param>
        public static void Debug(LoggingChannels channel, string message)
        {
            if (CanLog(channel))
            {
                UnityEngine.Debug.Log($"[ {channel} ] - {message}");
            }
        }

        /// <summary>
        /// Sends a log to the console with the severity of a Warning.
        /// </summary>
        /// <param name="channel">The type of channel to send the message via.</param>
        /// <param name="message">String value of the message to send.</param>
        public static void Warning(LoggingChannels channel, string message)
        {
            if (CanLog(channel))
            {
                UnityEngine.Debug.LogWarning($"[ {channel} ] - {message}");
            }
        }

        /// <summary>
        /// Sends a log to the console with the severity of a Error.
        /// </summary>
        /// <param name="channel">The type of channel to send the message via.</param>
        /// <param name="message">String value of the message to send.</param>
        public static void Error(LoggingChannels channel, string message)
        {
            if (CanLog(channel))
            {
                UnityEngine.Debug.LogError($"[ {channel} ] - {message}");
            }
        }

        /// <summary>
        /// Sends a log to the console with the severity of a Assertion.
        /// </summary>
        /// <param name="channel">The type of channel to send the message via.</param>
        /// <param name="message">String value of the message to send.</param>
        public static void Assertion(LoggingChannels channel, string message)
        {
            if (CanLog(channel))
            {
                UnityEngine.Debug.LogAssertion($"[ {channel} ] - {message}");
            }
        }

        /// <summary>
        /// Retrieves the currently enabled channels from the settings file.
        /// </summary>
        public static void GetSettingsFileData()
        {
            if (!SettingsFileExists())
            {
                CreateSettingsFile();
            }

            string data = File.ReadAllText(SettingsFilePath());
            m_enabledChannels = JsonUtility.FromJson<SaveData>(data).EnabledChannels;
        }

        /// <summary>
        /// Updates the settings file from the currently enabled channels list.
        /// </summary>
        public static void UpdateSettingsFile()
        {
            if (m_enabledChannels == null)
            {
                m_enabledChannels = new List<LoggingChannels>();
            }

            UpdateSettingsFile(m_enabledChannels);
        }

        /// <summary>
        /// Updates the settings file to contain the given list of channels.
        /// </summary>
        /// <param name="loggingChannels">The list of enabled channels.</param>
        public static void UpdateSettingsFile(List<LoggingChannels> loggingChannels)
        {
            string data = JsonUtility.ToJson(new SaveData() { EnabledChannels = loggingChannels });
            File.WriteAllText(SettingsFilePath(), data);
        }

        private static bool CanLog(LoggingChannels channel)
        {
            if (!SettingsFileExists())
            {
                CreateSettingsFile();
                UpdateSettingsFile();
            }

            GetSettingsFileData();
            return m_enabledChannels.Contains(channel);
        }

        private static string SettingsFilePath()
        {
            return Path.Combine(Application.persistentDataPath, LOGGING_SETTINGS_FILE_PATH, LOGGING_SETTINGS_FILE_NAME);
        }

        private static bool SettingsFileExists()
        {
            return File.Exists(SettingsFilePath());
        }

        private static void CreateSettingsFile()
        {
            if (!SettingsFileExists())
            {
                if (!Directory.Exists(Path.Combine(Application.persistentDataPath, LOGGING_SETTINGS_FILE_PATH)))
                {
                    Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, LOGGING_SETTINGS_FILE_PATH));
                }

                FileStream file = File.Create(SettingsFilePath());
                file.Close();
                m_enabledChannels = new List<LoggingChannels>();
                UpdateSettingsFile();
            }
        }
    }
}