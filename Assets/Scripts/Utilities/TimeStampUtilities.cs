using System;

namespace SymptomsPlease.Utilities.ExtensionMethods
{
    public static class TimeStampUtilities
    {
        public static DateTime EpochStart => new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long GetCurrentTimeStamp()
        {
            return GetTimeStamp(DateTime.UtcNow);
        }

        public static long GetTimeStamp(DateTime dateTime)
        {
            var epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            long currentEpochTime = (long)(dateTime - EpochStart).TotalSeconds;

            return currentEpochTime;
        }
    }
}