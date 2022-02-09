using System;
using UnityEngine;

public class DayTimer : MonoBehaviour
{
    public static event Action OnDayTimeComplete;

    [SerializeField] private float m_realtimeSecondsPerInGameDay = 120;
    [SerializeField] private float m_totalHouseInGameDay = 8;

    [Header("Display References")]
    [SerializeField] private Transform m_minuteHand = default;
    [SerializeField] private Transform m_hourHand = default;

    private const float HOUR_HAND_START_ROTATION = 90f;
    private const float HOUR_HAND_END_ROTATION = -150f;

    private const float MINUTE_HAND_START_ROTATION = 0;
    private const float MINUTE_HAND_END_ROTATION = -360;

    private float m_dayTime = 0;
    private bool m_isDayOver = false;

    private void Awake()
    {
        m_minuteHand.eulerAngles = new Vector3(0, 0, 0);
        m_hourHand.eulerAngles = new Vector3(0, 0, 90f);
    }

    private void Update()
    {
        if (!m_isDayOver)
        {
            m_dayTime += Time.deltaTime;

            float dayProgress = m_dayTime / m_realtimeSecondsPerInGameDay;

            if (dayProgress >= 1)
            {
                OnDayTimeComplete?.Invoke();
                m_isDayOver = true;
            }

            float hourRotation = (HOUR_HAND_START_ROTATION - HOUR_HAND_END_ROTATION) * dayProgress;
            m_hourHand.eulerAngles = new Vector3(0, 0, HOUR_HAND_START_ROTATION - hourRotation);

            float hourProgress = m_dayTime % (m_realtimeSecondsPerInGameDay / m_totalHouseInGameDay);

            float minuteRotation = (MINUTE_HAND_START_ROTATION - MINUTE_HAND_END_ROTATION) * (hourProgress / (m_realtimeSecondsPerInGameDay / m_totalHouseInGameDay));
            m_minuteHand.eulerAngles = new Vector3(0, 0, MINUTE_HAND_START_ROTATION - minuteRotation);
        }
    }
}