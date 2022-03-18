using UnityEngine;

namespace SymptomsPlease.Utilities.ExtensionMethods
{
    public static class ColorUtilities
    {
        public static Color GenerateRandomColor()
        {
            return new Color(
                    Random.Range(0f, 1f),
                    Random.Range(0f, 1f),
                    Random.Range(0f, 1f));
        }
    }
}