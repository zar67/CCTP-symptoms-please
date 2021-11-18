using System;
using TMPro;
using UnityEngine;

public class DayTimerDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_timerText = default;

    private void OnEnable()
    {
        DayCycle.OnTimerValueChanged += OnTimerValueChanged;
    }

    private void OnDisable()
    {
        DayCycle.OnTimerValueChanged -= OnTimerValueChanged;
    }

    private void OnTimerValueChanged(int timer)
    {
        var timeSpan = TimeSpan.FromSeconds(timer);
        m_timerText.text = timeSpan.ToString(@"mm\:ss");
    }
}