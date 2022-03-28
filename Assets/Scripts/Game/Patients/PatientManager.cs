using DG.Tweening;
using SymptomsPlease.Transitions;
using SymptomsPlease.UI.Popups;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PatientManager : MonoBehaviour
{
    public struct PatientSeenData
    {
        public bool HasHelpedPatient;
        public PatientData PatientData;
        public ActionType ActionTaken;
        public ActionEffectiveness ActionEffectiveness;
        public List<DayEventType> TriggeredEvents;
        public float ScoreGained;
    }

    public static event Action<PatientSeenData> OnPatientSeen;
    public static event Action<PatientData> OnNextPatient;

    public static PatientData CurrentPatient { get; private set; }

    public static int PatientSeenInDay { get; private set; } = 0;
    public static int PatientsHelpedInDay { get; private set; } = 0;

    public static int PatientsStrikedOutInDay { get; private set; } = 0;

    public static ActionType CurrentAction { get; private set; }

    private static float m_scoreMultiplier = 10;

    [SerializeField] private RectTransform m_patientHolder = default;
    [SerializeField] private AvatarDisplay m_patientDisplay = default;

    [Header("Tween Values")]
    [SerializeField] private float m_tweenAnimationDuration = 1.5f;
    [SerializeField] private Vector3 m_tweenStartPosition = new Vector3(-3.0f, 0.0f, 0.0f);
    [SerializeField] private Vector3 m_tweenCenteredPosition = new Vector3(0.0f, 0.0f, 0.0f);
    [SerializeField] private Vector3 m_tweenEndPosition = new Vector3(3.0f, 0.0f, 0.0f);

    [Header("Patients")]
    [SerializeField] private float m_delayBetweenPatients = 0.5f;
    [SerializeField] private AvatarData m_avatarData = default;
    [SerializeField] private AllAfflictionDatas m_afflictionDatas = default;
    [SerializeField] private List<string> m_validPatientNames = new List<string>();

    [Header("STI Testing")]
    [SerializeField] private STITestResults m_stiResultsDisplay = default;

    [Header("FTUE")]
    [SerializeField] private PopupData m_popupData = default;
    [SerializeField] private string m_actionFTUEPopup = "popup_ftue_actions";
    [SerializeField] private string m_testKitFTUEPopup = "popup_ftue_testing";
    [SerializeField] private string m_adviceFTUEPopup = "popup_ftue_advice";
    [SerializeField] private AfflictionData m_initialAffliction = default;
    [SerializeField] private AfflictionData m_initialAdviceAffliction = default;

    private static int m_currentPatientIndex = default;

    private Tween m_moveOutTween = null;
    private Tween m_moveInTween = null;

    private void Awake()
    {
        PatientSeenInDay = 0;
        PatientsHelpedInDay = 0;
        PatientsStrikedOutInDay = 0;

        TransitionManager.OnTransitionComplete.Subscribe(OnTransitionComplete);
    }

    private void OnEnable()
    {
        ActionObject.OnDraggableOnPatient += OnPlayerAction;
        DayTimer.OnDayTimerComplete += OnDayTimerComplete;
    }

    private void OnDisable()
    {
        ActionObject.OnDraggableOnPatient -= OnPlayerAction;
        DayTimer.OnDayTimerComplete -= OnDayTimerComplete;
    }

    private void OnTransitionComplete(TransitionData data)
    {
        ShowNextPatient();
        TransitionManager.OnTransitionComplete.UnSubscribe(OnTransitionComplete);
    }

    private void OnPlayerAction(ActionObject action)
    {
        if (DayTimer.IsTimerComplete)
        {
            return;
        }

        CurrentAction = action.ActionType;

        ActionEffectiveness effectiveness = ActionEffectiveness.WORST;
        if (action.ActionType == ActionType.GIVE_ADVICE)
        {
            var advice = action as AdviceObject;
            effectiveness = CurrentPatient.AfflictionData.GetAdviceEffectiveness(advice.Advice);
        }
        else
        {
            effectiveness = CurrentPatient.AfflictionData.GetActionEffectiveness(action.ActionType);
        }

        int effectivenessIntValue = (int)effectiveness;

        CurrentPatient.PreviousActions.Add(action.ActionType);

        int scoreGained = (int)((effectivenessIntValue - 2) * m_scoreMultiplier);
        DayCycle.IncreaseScore(scoreGained);

        if (action.ActionType == ActionType.GIVE_BLOOD_TEST_KIT ||
            action.ActionType == ActionType.GIVE_SWAB_TEST_KIT ||
            action.ActionType == ActionType.GIVE_URINE_TEST_KIT)
        {
            CancelTweens(false);
            m_moveInTween = m_patientHolder.DOAnchorPos(m_tweenStartPosition, m_tweenAnimationDuration).OnComplete(DelayPatientSTITest);
            return;
        }

        HandleCompletePatient(effectiveness, scoreGained);
    }

    private void OnDayTimerComplete()
    {
        DayEventsManager.DayEvents.Add(new NewAppointmentEvent()
        {
            EventType = DayEventType.PATIENT_BOOKS_NEW_APPOINTMENT,
            PatientID = CurrentPatient.ID,
            NewAppointmentDay = GameData.DayNumber + 1
        });
    }

    private void HandleCompletePatient(ActionEffectiveness effectiveness, int scoreGained)
    {
        var triggeredEvents = new List<DayEventType>();

        if (effectiveness < ActionEffectiveness.BEST)
        {
            CurrentPatient.PlayerStrikes++;

            if (CurrentPatient.PlayerStrikes < 3)
            {
                DayEventsManager.DayEvents.Add(new NewAppointmentEvent()
                {
                    EventType = DayEventType.PATIENT_BOOKS_NEW_APPOINTMENT,
                    PatientID = CurrentPatient.ID,
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

            GameData.Patients.Remove(CurrentPatient.ID);
        }

        var patientSeenData = new PatientSeenData()
        {
            HasHelpedPatient = (int)effectiveness > 2,
            PatientData = CurrentPatient,
            ActionTaken = CurrentAction,
            ActionEffectiveness = effectiveness,
            TriggeredEvents = triggeredEvents,
            ScoreGained = scoreGained
        };

        m_currentPatientIndex++;
        PatientSeenInDay++;
        if ((int)effectiveness > 2)
        {
            PatientsHelpedInDay++;
        }

        OnPatientSeen?.Invoke(patientSeenData);

        m_moveOutTween = m_patientHolder.DOAnchorPos(m_tweenEndPosition, m_tweenAnimationDuration).OnComplete(DelayNextPatient);
    }

    private PatientData GenerateNewPatient()
    {
        if ((!FTUEManager.IsFTUETypeHandled(EFTUEType.SEEN_ACTIONS_FTUE) && PatientSeenInDay == 0) || 
            (!FTUEManager.IsFTUETypeHandled(EFTUEType.SEEN_TEST_KIT_RESULTS_FTUE) && PatientSeenInDay == 1))
        {
            var ftuePatient = new PatientData()
            {
                ID = GameData.Patients.Count,
                Name = m_validPatientNames[Random.Range(0, m_validPatientNames.Count)],
                PlayerStrikes = 0,
                AppointmentSummary = m_initialAffliction.GetAfflictionSummary(),
                AfflictionData = m_initialAffliction,
                AvatarData = m_avatarData.GenerateRandomData()
            };

            GameData.Patients.Add(ftuePatient.ID, ftuePatient);
            return ftuePatient;
        }
        
        if (!FTUEManager.IsFTUETypeHandled(EFTUEType.SEEN_ADVICE_FTUE) && PatientSeenInDay == 2)
        {
            var ftuePatient = new PatientData()
            {
                ID = GameData.Patients.Count,
                Name = m_validPatientNames[Random.Range(0, m_validPatientNames.Count)],
                PlayerStrikes = 0,
                AppointmentSummary = m_initialAdviceAffliction.GetAfflictionSummary(),
                AfflictionData = m_initialAdviceAffliction,
                AvatarData = m_avatarData.GenerateRandomData()
            };

            GameData.Patients.Add(ftuePatient.ID, ftuePatient);
            return ftuePatient;
        }

        if (DayEventsManager.DayEvents.Count != 0)
        {
            foreach (DayEvent dayEvent in DayEventsManager.DayEvents)
            {
                if (dayEvent is NewAppointmentEvent appointmentEvent)
                {
                    PatientData eventPatient = GameData.Patients[appointmentEvent.PatientID];
                    if (GameData.DayNumber == appointmentEvent.NewAppointmentDay &&
                        ModificationsManager.IsTopicActive(eventPatient.AfflictionData.Topic))
                    {
                        DayEventsManager.DayEvents.Remove(dayEvent);
                        return eventPatient;
                    }
                }
            }
        }

        int index = Random.Range(0, m_afflictionDatas.AfflictionsCount);

        while (!ModificationsManager.IsTopicActive(m_afflictionDatas.GetAfflictionAtIndex(index).Topic))
        {
            index = Random.Range(0, m_afflictionDatas.AfflictionsCount);
        }

        var newPatient = new PatientData()
        {
            ID = GameData.NextPatientID,
            Name = m_validPatientNames[Random.Range(0, m_validPatientNames.Count)],
            PlayerStrikes = 0,
            AppointmentSummary = m_afflictionDatas.GetAfflictionAtIndex(index).GetAfflictionSummary(),
            AfflictionData = m_afflictionDatas.GetAfflictionAtIndex(index),
            AvatarData = m_avatarData.GenerateRandomData()
        };

        GameData.Patients.Add(newPatient.ID, newPatient);
        GameData.NextPatientID++;

        return newPatient;
    }

    private void ShowNextPatient()
    {
        if (DayTimer.IsTimerComplete)
        {
            return;
        }

        CancelTweens();
        CurrentPatient = GenerateNewPatient();

        m_patientHolder.anchoredPosition = m_tweenStartPosition;
        m_patientDisplay.ShowAvatar(CurrentPatient.AvatarData);
        m_moveInTween = m_patientHolder.DOAnchorPos(m_tweenCenteredPosition, m_tweenAnimationDuration).OnComplete(OnPatientCentered);

        OnNextPatient?.Invoke(CurrentPatient);
    }
    private void OnPatientCentered()
    {
        if (!FTUEManager.IsFTUETypeHandled(EFTUEType.SEEN_ACTIONS_FTUE) && PatientSeenInDay == 0)
        {
            m_popupData.OpenPopup(m_actionFTUEPopup);
            FTUEManager.HandleFTUEType(EFTUEType.SEEN_ACTIONS_FTUE);
        }
        else if (!FTUEManager.IsFTUETypeHandled(EFTUEType.SEEN_TESTING_FTUE) && PatientSeenInDay == 1)
        {
            m_popupData.OpenPopup(m_testKitFTUEPopup);
            FTUEManager.HandleFTUEType(EFTUEType.SEEN_TESTING_FTUE);
        }
        else if (!FTUEManager.IsFTUETypeHandled(EFTUEType.SEEN_ADVICE_FTUE) && PatientSeenInDay == 2)
        {
            m_popupData.OpenPopup(m_adviceFTUEPopup);
            FTUEManager.HandleFTUEType(EFTUEType.SEEN_ADVICE_FTUE);
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