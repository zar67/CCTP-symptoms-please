using System;
using UnityEngine;
using UnityEngine.UI;

namespace SymptomsPlease.Debugger
{
    public class ToggleOption : Option
    {
        public event Action<bool> OnToggled;

        [SerializeField] private Toggle m_toggle = default;

        private void OnEnable()
        {
            m_toggle.onValueChanged.AddListener(OnTogglePressed);
        }

        private void OnDisable()
        {
            m_toggle.onValueChanged.RemoveListener(OnTogglePressed);
        }

        private void OnTogglePressed(bool value)
        {
            OnToggled?.Invoke(value);
        }
    }
}