using DG.Tweening;
using SymptomsPlease.SceneManagement;
using SymptomsPlease.Transitions;
using SymptomsPlease.UI.Panels;
using System;
using UnityEngine;

public class DayCycle : MonoBehaviour
{
    public static event Action<int> OnTimerValueChanged;

    public static int PatientSeenInDay => m_patientsSeen;
    public static int PatientsHelpedInDay => m_patientsHelped;

    [SerializeField] private int m_dayLengthInMinutes = 1;

    [Header("Day End Transition Values")]
    [SerializeField] private SceneData m_sceneData = default;
    [SerializeField] private PanelsData m_panelsData = default;
    [SerializeField] private string m_dayEndPanel = "panel_day_end";
    [SerializeField] private SceneTransitionData m_sceneTransitionData = default;

    private static int m_patientsSeen = 0;
    private static int m_patientsHelped = 0;

    private float m_dayTimer = 0;
    private float m_previousTimerValue = 0;

    private void Awake()
    {
        m_previousTimerValue = m_dayLengthInMinutes;
        m_patientsSeen = 0;
        m_patientsHelped = 0;
    }

    private void OnEnable()
    {
        PatientManager.OnPatientSeen += OnPatientSeen;
    }

    private void OnDisable()
    {
        PatientManager.OnPatientSeen -= OnPatientSeen;
    }

    private void Update()
    {
        if (m_previousTimerValue == 0)
        {
            return;
        }

        m_dayTimer += Time.deltaTime;

        if (m_dayLengthInMinutes - (int)m_dayTimer != m_previousTimerValue)
        {
            int timerValue = (m_dayLengthInMinutes * 60) - (int)m_dayTimer;

            if (timerValue < 0)
            {
                timerValue = 0;
            }

            OnTimerValueChanged?.Invoke(timerValue);
            m_previousTimerValue = timerValue;
        }
    }

    private void OnPatientSeen(bool helped)
    {
        m_patientsSeen++;

        if (helped)
        {
            m_patientsHelped++;
        }

        if (m_dayTimer >= m_dayLengthInMinutes * 60)
        {
            DOTween.Clear(true);
            m_panelsData.SetupInitialPanel(m_dayEndPanel);
            m_sceneTransitionData.State = TransitionData.TransitionState.OUT;
            m_sceneData.TransitionToScene(m_sceneTransitionData);
        }
    }
}