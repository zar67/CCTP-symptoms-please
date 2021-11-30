using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PatientManager : MonoBehaviour
{
    public static event Action<bool> OnPatientSeen;
    public static event Action<PatientData> OnNextPatient;

    public static List<PatientData> PatientsInDay { get; private set; } = new List<PatientData>();
    public static int PatientSeenInDay { get; private set; } = 0;
    public static int PatientsHelpedInDay { get; private set; } = 0;

    [SerializeField] private GameObject m_patientHolder = default;
    [SerializeField] private float m_tweenAnimationDuration = 1.5f;

    [SerializeField] private float m_actionScoreMultiplier = 10.0f;

    [Header("Patients")]
    [SerializeField] private int m_numberPatientsInDay = 10;
    [SerializeField] private PatientData[] m_patientDatas = default;
    private bool m_isDayOver = false;
    private int m_currentPatientIndex = default;

    private Vector3 m_tweenStartPosition = new Vector3(-3.0f, 0.0f, 0.0f);
    private Vector3 m_tweenCenteredPosition = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector3 m_tweenEndPosition = new Vector3(3.0f, 0.0f, 0.0f);

    private void Awake()
    {
        GeneratePatients();
        ShowNextPatient();

        PatientSeenInDay = 0;
        PatientsHelpedInDay = 0;
    }

    private void OnEnable()
    {
        ActionObject.OnDraggableOnPatient += OnPlayerAction;
    }

    private void OnDisable()
    {
        ActionObject.OnDraggableOnPatient -= OnPlayerAction;
    }

    private void OnPlayerAction(ActionObject action)
    {
        ActionEffectiveness effectiveness = PatientsInDay[m_currentPatientIndex].AfflictionData.GetActionEffectiveness(action.ActionType);

        if (effectiveness < ActionEffectiveness.NEUTRAL)
        {
            DayEventsManager.DayEvents.Add(new NewAppointmentEvent()
            {
                EventType = DayEventType.PATIENT_BOOKS_NEW_APPOINTMENT,
                Patient = PatientsInDay[m_currentPatientIndex],
                NewAppointmentDay = GameData.DayNumber + 1
            });
        }

        if (effectiveness < ActionEffectiveness.BAD)
        {
            DayEventsManager.AddEvent(DayEventType.PATIENT_COMPLAINS);
        }

        if (effectiveness > ActionEffectiveness.GOOD)
        {
            DayEventsManager.AddEvent(DayEventType.PATIENT_CURED);
        }

        PatientSeenInDay++;

        if ((int)effectiveness > 2)
        {
            PatientsHelpedInDay++;
        }

        DayCycle.IncreaseScore(((int)effectiveness - 2) * m_actionScoreMultiplier);

        m_currentPatientIndex++;
        m_isDayOver = m_currentPatientIndex >= PatientsInDay.Count; 
        OnPatientSeen?.Invoke(m_isDayOver);

        m_patientHolder.transform.DOMove(m_tweenEndPosition, m_tweenAnimationDuration).OnComplete(ShowNextPatient);
    }

    private void GeneratePatients()
    {
        PatientsInDay = new List<PatientData>();
        int patientCount = m_numberPatientsInDay;

        var usedEvents = new List<DayEvent>();
        foreach (DayEvent dayEvent in DayEventsManager.DayEvents)
        {
            if (patientCount == 0)
            {
                break;
            }

            if (dayEvent is NewAppointmentEvent appointmentEvent)
            {
                if (GameData.DayNumber == appointmentEvent.NewAppointmentDay)
                {
                    PatientsInDay.Add(appointmentEvent.Patient);
                    usedEvents.Add(dayEvent);
                    patientCount--;
                }
            }
        }

        foreach (DayEvent dayEvent in usedEvents)
        {
            DayEventsManager.DayEvents.Remove(dayEvent);
        }

        for (int i = 0; i < patientCount; i++)
        {
            int index = UnityEngine.Random.Range(0, m_patientDatas.Length);
            PatientsInDay.Add(m_patientDatas[index]);
        }
    }

    private void ShowNextPatient()
    {
        if (!m_isDayOver)
        {
            m_patientHolder.transform.position = m_tweenStartPosition;
            m_patientHolder.transform.DOMove(m_tweenCenteredPosition, m_tweenAnimationDuration);

            OnNextPatient?.Invoke(PatientsInDay[m_currentPatientIndex]);
        }
    }
}