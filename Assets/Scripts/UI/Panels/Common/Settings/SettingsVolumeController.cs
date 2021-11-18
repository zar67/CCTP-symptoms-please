using UnityEngine;
using UnityEngine.Audio;

namespace SymptomsPlease.UI.Panels.Common.Settings
{
    public class SettingsVolumeController : MonoBehaviour
    {
        [SerializeField] private AudioMixer m_audioMixer = default;
        [SerializeField] private string m_mixerGroup = "Master";

        public void SetVolume(float value)
        {
            m_audioMixer.SetFloat(m_mixerGroup, Mathf.Log10(value) * 20);
        }

        public void IncreaseVolume(float amount)
        {
            m_audioMixer.GetFloat(m_mixerGroup, out float value);
            float normalisedValue = Mathf.Pow(10, value / 20);
            m_audioMixer.SetFloat(m_mixerGroup, Mathf.Log10(GetNormalisedValue(value) - amount) * 20);
        }

        public void DescreaseVolume(float amount)
        {
            m_audioMixer.GetFloat(m_mixerGroup, out float value);
            m_audioMixer.SetFloat(m_mixerGroup, Mathf.Log10(GetNormalisedValue(value) - amount) * 20);
        }

        private float GetNormalisedValue(float value)
        {
            return Mathf.Pow(10, value / 20);
        }
    }
}