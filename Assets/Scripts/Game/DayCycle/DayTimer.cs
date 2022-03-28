using SymptomsPlease.Transitions;
using SymptomsPlease.UI.Popups;
using System;
using UnityEngine;

public class DayTimer : MonoBehaviour
{
    public static event Action OnDayTimerComplete;

    public static bool IsTimerComplete => m_isTimerComplete;

    [SerializeField] private float m_realtimeSecondsPerInGameDay = 120;
    [SerializeField] private float m_totalHouseInGameDay = 8;

    [Header("Display References")]
    [SerializeField] private Transform m_minuteHand = default;
    [SerializeField] private Transform m_hourHand = default;

    [Header("Hand Rotation Values")]
    [SerializeField] private float m_hourHandStartRotation = 0;
    [SerializeField] private float m_hourHandEndRotation = -360;

    [SerializeField] private float m_minuteHandStartRotation = 0;
    [SerializeField] private float m_minuteHandEndRotation = -360;

    [Header("FTUE")]
    [SerializeField] private PopupData m_popupData = default;
    [SerializeField] private string m_timerFTUEPopup = "popup_ftue_timer";

    private float m_dayTime = 0;
    private static bool m_isTimerComplete = false;

    private void Awake()
    {
        m_minuteHand.eulerAngles = new Vector3(0, 0, 0);
        m_hourHand.eulerAngles = new Vector3(0, 0, 0);

        TransitionManager.OnTransitionComplete.Subscribe(OnTransitionComplete);
    }

    private void Update()
    {
        if (!m_isTimerComplete)
        {
            m_dayTime += Time.deltaTime;

            float dayProgress = m_dayTime / m_realtimeSecondsPerInGameDay;

            if (dayProgress >= 1)
            {
                OnDayTimerComplete?.Invoke();
                m_isTimerComplete = true;
            }

            float hourRotation = (m_hourHandStartRotation - m_hourHandEndRotation) * dayProgress;
            m_hourHand.eulerAngles = new Vector3(0, 0, m_hourHandStartRotation - hourRotation);

            float hourProgress = m_dayTime % (m_realtimeSecondsPerInGameDay / m_totalHouseInGameDay);

            float minuteRotation = (m_minuteHandStartRotation - m_minuteHandEndRotation) * (hourProgress / (m_realtimeSecondsPerInGameDay / m_totalHouseInGameDay));
            m_minuteHand.eulerAngles = new Vector3(0, 0, m_minuteHandStartRotation - minuteRotation);
        }
    }

    private void OnTransitionComplete(TransitionData data)
    {
        if (!FTUEManager.IsFTUETypeHandled(EFTUEType.SEEN_TIMER_FTUE))
        {
            m_popupData.OpenPopup(m_timerFTUEPopup);
            FTUEManager.HandleFTUEType(EFTUEType.SEEN_TIMER_FTUE);
        }

        TransitionManager.OnTransitionComplete.UnSubscribe(OnTransitionComplete);
    }
}