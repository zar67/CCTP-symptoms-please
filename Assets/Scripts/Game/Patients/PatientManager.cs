using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

public class PatientManager : MonoBehaviour
{
    public struct PatientSeenData
    {
        public bool IsEndOfDay;
        public bool HasHelpedPatient;
        public ActionEffectiveness ActionEffectiveness;
        public List<DayEventType> TriggeredEvents;
        public float ScoreGained;
    }

    public static event Action<PatientSeenData> OnPatientSeen;
    public static event Action<PatientData> OnNextPatient;

    public static PatientData CurrentPatient => GameData.Patients[PatientsInDay[m_currentPatientIndex]];

    public static List<int> PatientsInDay { get; private set; } = new List<int>();
    public static int PatientSeenInDay { get; private set; } = 0;
    public static int PatientsHelpedInDay { get; private set; } = 0;

    public static int PatientsStrikedOutInDay { get; private set; } = 0;

    public static ActionType CurrentAction { get; private set; }

    [SerializeField] private RectTransform m_patientHolder = default;
    [SerializeField] private AvatarDisplay m_patientDisplay = default;

    [SerializeField] private float m_actionScoreMultiplier = 10.0f;

    [Header("Tween Values")]
    [SerializeField] private float m_tweenAnimationDuration = 1.5f;
    [SerializeField] private Vector3 m_tweenStartPosition = new Vector3(-3.0f, 0.0f, 0.0f);
    [SerializeField] private Vector3 m_tweenCenteredPosition = new Vector3(0.0f, 0.0f, 0.0f);
    [SerializeField] private Vector3 m_tweenEndPosition = new Vector3(3.0f, 0.0f, 0.0f);

    [Header("Patients")]
    [SerializeField] private int m_numberPatientsInDay = 10;
    [SerializeField] private float m_delayBetweenPatients = 0.5f;
    [SerializeField] private AvatarData m_avatarData = default;
    [SerializeField] private AfflictionData[] m_afflictionDatas = default;
    [SerializeField] private List<string> m_validPatientNames = new List<string>();

    [Header("STI Testing")]
    [SerializeField] private STITestResults m_stiResultsDisplay = default;

    private static bool m_isDayOver = false;
    private static int m_currentPatientIndex = default;

    private Tween m_moveOutTween = null;
    private Tween m_moveInTween = null;

    private void Awake()
    {
        m_isDayOver = false;
        PatientSeenInDay = 0;
        PatientsHelpedInDay = 0;
        PatientsStrikedOutInDay = 0;

        GeneratePatients();
        ShowNextPatient();
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
        CurrentAction = action.ActionType;

        PatientData currentPatient = GameData.Patients[PatientsInDay[m_currentPatientIndex]];
        ActionEffectiveness effectiveness = currentPatient.AfflictionData.GetActionEffectiveness(action.ActionType);
        int effectivenessIntValue = (int)effectiveness;

        currentPatient.PreviousActions.Add(action.ActionType);

        int scoreGained = (int)((effectivenessIntValue - 2) * m_actionScoreMultiplier);
        DayCycle.IncreaseScore(scoreGained);

        if (action.ActionType == ActionType.GIVE_BLOOD_TEST_KIT ||
            action.ActionType == ActionType.GIVE_SWAB_TEST_KIT ||
            action.ActionType == ActionType.GIVE_URINE_TEST_KIT)
        {
            CancelTweens(false);
            m_moveInTween = m_patientHolder.DOAnchorPos(m_tweenStartPosition, m_tweenAnimationDuration).OnComplete(DelayPatientSTITest);
            return;
        }

        var triggeredEvents = new List<DayEventType>();
        if (effectiveness < ActionEffectiveness.BEST)
        {
            currentPatient.PlayerStrikes++;

            if (currentPatient.PlayerStrikes < 3)
            {
                DayEventsManager.DayEvents.Add(new NewAppointmentEvent()
                {
                    EventType = DayEventType.PATIENT_BOOKS_NEW_APPOINTMENT,
                    PatientID = currentPatient.ID,
                    NewAppointmentDay = GameData.DayNumber + 1
                });
                triggeredEvents.Add(DayEventType.PATIENT_BOOKS_NEW_APPOINTMENT);
            }
            else
            {
                PatientsStrikedOutInDay++;
                DayCycle.IncreaseScore(-30);
            }
        }

        if (effectiveness < ActionEffectiveness.BAD)
        {
            DayEventsManager.AddEvent(DayEventType.PATIENT_COMPLAINS);
            triggeredEvents.Add(DayEventType.PATIENT_COMPLAINS);
        }

        if (effectiveness > ActionEffectiveness.GOOD)
        {
            DayEventsManager.AddEvent(DayEventType.PATIENT_CURED);
            triggeredEvents.Add(DayEventType.PATIENT_CURED);
        }

        PatientSeenInDay++;

        bool helped = effectivenessIntValue > 2;
        if (effectivenessIntValue > 2)
        {
            PatientsHelpedInDay++;
        }

        m_currentPatientIndex++;
        m_isDayOver = m_currentPatientIndex >= PatientsInDay.Count;

        var patientSeenData = new PatientSeenData()
        {
            IsEndOfDay = m_isDayOver,
            HasHelpedPatient = helped,
            ActionEffectiveness = effectiveness,
            TriggeredEvents = triggeredEvents,
            ScoreGained = scoreGained
        };

        OnPatientSeen?.Invoke(patientSeenData);

        if (m_isDayOver)
        {
            CancelTweens();
        }
        else
        {
            m_moveOutTween = m_patientHolder.DOAnchorPos(m_tweenEndPosition, m_tweenAnimationDuration).OnComplete(DelayNextPatient);
        }
    }

    private void GeneratePatients()
    {
        m_currentPatientIndex = 0;
        PatientsInDay = new List<int>();
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
                PatientData eventPatient = GameData.Patients[appointmentEvent.PatientID];
                if (GameData.DayNumber == appointmentEvent.NewAppointmentDay &&
                    ModificationsManager.IsTopicActive(eventPatient.AfflictionData.Topic))
                {
                    PatientsInDay.Add(eventPatient.ID);
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
            int index = Random.Range(0, m_afflictionDatas.Length);

            while (!ModificationsManager.IsTopicActive(m_afflictionDatas[index].Topic))
            {
                index = Random.Range(0, m_afflictionDatas.Length);
            }

            var newPatient = new PatientData()
            {
                ID = GameData.Patients.Count,
                Name = m_validPatientNames[Random.Range(0, m_validPatientNames.Count)],
                PlayerStrikes = 0,
                AppointmentSummary = m_afflictionDatas[index].GetAfflictionSummary(),
                AfflictionData = m_afflictionDatas[index],
                AvatarData = m_avatarData.GenerateRandomData()
            };

            GameData.Patients.Add(newPatient.ID, newPatient);
            PatientsInDay.Add(newPatient.ID);
        }
    }

    private void ShowNextPatient()
    {
        if (!m_isDayOver)
        {
            CancelTweens();
            PatientData nextPatient = GameData.Patients[PatientsInDay[m_currentPatientIndex]];

            m_patientHolder.anchoredPosition = m_tweenStartPosition;
            m_patientDisplay.ShowAvatar(nextPatient.AvatarData);
            m_moveInTween = m_patientHolder.DOAnchorPos(m_tweenCenteredPosition, m_tweenAnimationDuration);

            OnNextPatient?.Invoke(nextPatient);
        }
    }

    private void CancelTweens(bool setPosition = true)
    {
        if (m_moveOutTween != null)
        {
            m_moveOutTween.Kill();
            m_moveOutTween = null;

            if (setPosition)
            {
                m_patientHolder.anchoredPosition = m_tweenStartPosition;
            }
        }

        if (m_moveInTween != null)
        {
            m_moveInTween.Kill();
            m_moveInTween = null;

            if (setPosition)
            {
                m_patientHolder.anchoredPosition = m_tweenEndPosition;
            }
        }
    }

    private void DelayNextPatient()
    {
        StartCoroutine(DelayPatient());
    }

    private IEnumerator DelayPatient()
    {
        yield return new WaitForSeconds(m_delayBetweenPatients);
        ShowNextPatient();
    }

    private void DelayPatientSTITest()
    {
        StartCoroutine(PatientSTITest());
    }

    private IEnumerator PatientSTITest()
    {
        yield return new WaitForSeconds(m_delayBetweenPatients);

        m_moveInTween = m_patientHolder.DOAnchorPos(m_tweenCenteredPosition, m_tweenAnimationDuration);
        m_stiResultsDisplay.ShowResults(CurrentPatient);
    }
}