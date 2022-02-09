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

    private float m_dayTime = 0;

    private const float HOUR_HAND_START_ROTATION = 90f;
    private const float HOUR_HAND_END_ROTATION = -150f;

    private const float MINUTE_HAND_START_ROTATION = 0;
    private const float MINUTE_HAND_END_ROTATION = -360;

    private float m_minuteTimer = 0;

    private void Awake()
    {
        m_minuteHand.eulerAngles = new Vector3(0, 0, 0);
        m_hourHand.eulerAngles = new Vector3(0, 0, 90f);
    }

    private void Update()
    {
        m_dayTime += Time.deltaTime / m_realtimeSecondsPerInGameDay;

        if (m_dayTime >= 1)
        {
            OnDayTimeComplete?.Invoke();
            m_dayTime = 1;
        }

        float hourRotation = (HOUR_HAND_START_ROTATION - HOUR_HAND_END_ROTATION) * m_dayTime;
        m_hourHand.eulerAngles = new Vector3(0, 0, HOUR_HAND_START_ROTATION - hourRotation);

        float hourProgress = (m_dayTime * m_realtimeSecondsPerInGameDay) % (m_realtimeSecondsPerInGameDay / m_totalHouseInGameDay);

        float minuteRotation = (MINUTE_HAND_START_ROTATION - MINUTE_HAND_END_ROTATION) * (hourProgress / (m_realtimeSecondsPerInGameDay / m_totalHouseInGameDay));
        m_minuteHand.eulerAngles = new Vector3(0, 0, MINUTE_HAND_START_ROTATION - minuteRotation);
    }
}