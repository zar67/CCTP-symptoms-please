using DG.Tweening;
using System;
using UnityEngine;

public class PatientManager : MonoBehaviour
{
    public static event Action OnPatientSeen;
    public static event Action<PatientData> OnNextPatient;

    [SerializeField] private GameObject m_patientHolder = default;
    [SerializeField] private float m_tweenAnimationDuration = 1.5f;

    [Header("Patients")]
    [SerializeField] private PatientData[] m_patientDatas = default;

    private bool m_isDayOver = false;
    private PatientData m_currentPatient = default;

    private Vector3 m_tweenStartPosition = new Vector3(-3.0f, 0.0f, 0.0f);
    private Vector3 m_tweenCenteredPosition = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector3 m_tweenEndPosition = new Vector3(3.0f, 0.0f, 0.0f);

    private void Awake()
    {
        GenerateNextPatient();
    }

    private void OnEnable()
    {
        ActionObject.OnDraggableOnPatient += OnPlayerAction;
        DayCycle.OnTimerValueChanged += OnTimerValueChanged;
    }

    private void OnDisable()
    {
        ActionObject.OnDraggableOnPatient -= OnPlayerAction;
        DayCycle.OnTimerValueChanged -= OnTimerValueChanged;
    }

    private void OnPlayerAction(ActionObject action)
    {
        // Handle Action Done
        Debug.Log(action.ActionType);

        OnPatientSeen?.Invoke();

        m_patientHolder.transform.DOMove(m_tweenEndPosition, m_tweenAnimationDuration).OnComplete(GenerateNextPatient);
    }

    private void GenerateNextPatient()
    {
        if (!m_isDayOver)
        {
            m_patientHolder.transform.position = m_tweenStartPosition;
            m_patientHolder.transform.DOMove(m_tweenCenteredPosition, m_tweenAnimationDuration);

            int index = UnityEngine.Random.Range(0, m_patientDatas.Length);
            m_currentPatient = m_patientDatas[index];

            OnNextPatient?.Invoke(m_currentPatient);
        }
    }

    private void OnTimerValueChanged(int timer)
    {
        if (timer == 0)
        {
            m_isDayOver = true;
        }
    }
}