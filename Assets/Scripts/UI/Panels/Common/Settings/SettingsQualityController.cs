using UnityEngine;

namespace SymptomsPlease.UI.Panels.Common.Settings
{
    public class SettingsQualityController : MonoBehaviour
    {
        public void SetQuality(int qualityIndex)
        {
            QualitySettings.SetQualityLevel(qualityIndex);
        }
    }
}