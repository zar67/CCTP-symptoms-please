using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SymptomsPlease.Debugger
{
    public class ActionOption : Option
    {
        public event Action OnAction;

        [SerializeField] private Button m_actionButton = default;

        private void OnEnable()
        {
            m_actionButton.onClick.AddListener(OnButtonPressed);
        }

        private void OnDisable()
        {
            m_actionButton.onClick.RemoveListener(OnButtonPressed);
        }

        private void OnButtonPressed()
        {
            OnAction?.Invoke();
        }
    }
}