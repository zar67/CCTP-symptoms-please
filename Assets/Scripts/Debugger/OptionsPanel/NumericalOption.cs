using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SymptomsPlease.Debugger
{
    public class NumericalOption : Option
    {
        public event Action<int> OnValueChanged;

        [SerializeField] private TextMeshProUGUI m_valueText = default;
        [SerializeField] private Button m_decrementButton = default;
        [SerializeField] private Button m_incementButton = default;

        private int m_value;

        public void SetDefaultValue(int value)
        {
            m_value = value;
            OnValueChanged?.Invoke(m_value);
            m_valueText.text = m_value.ToString();
        }

        private void OnEnable()
        {
            m_decrementButton.onClick.AddListener(OnValueDecremented);
            m_incementButton.onClick.AddListener(OnValueIncremented);
        }

        private void OnDisable()
        {
            m_decrementButton.onClick.RemoveListener(OnValueDecremented);
            m_incementButton.onClick.RemoveListener(OnValueIncremented);
        }

        private void OnValueDecremented()
        {
            m_value--;
            OnValueChanged?.Invoke(m_value);
            m_valueText.text = m_value.ToString();
        }

        private void OnValueIncremented()
        {
            m_value++;
            OnValueChanged?.Invoke(m_value);
            m_valueText.text = m_value.ToString();
        }
    }
}