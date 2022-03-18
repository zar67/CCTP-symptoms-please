using System;
using UnityEngine;

namespace SymptomsPlease.Debugging.Logging
{
    public class LoggerInitialiser : MonoBehaviour
    {
        public void EnableChannel(string[] parameters)
        {
            if (Enum.TryParse(parameters[0], out LoggingChannels channel))
            {
                CustomLogger.EnableChannel(channel);
            }
            else
            {
                CustomLogger.Debug(LoggingChannels.DebugCommands, $"Could not find logging channel: {parameters[0]}");
            }
        }

        public void DisableChannel(string[] parameters)
        {
            if (Enum.TryParse(parameters[0], out LoggingChannels channel))
            {
                CustomLogger.DisableChannel(channel);
            }
            else
            {
                CustomLogger.Debug(LoggingChannels.DebugCommands, $"Could not find logging channel: {parameters[0]}");
            }
        }
    }
}