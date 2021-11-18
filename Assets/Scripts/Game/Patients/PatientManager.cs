using DG.Tweening;
using System;
using UnityEngine;

public class PatientManager : MonoBehaviour
{
    public static event Action OnPatientSeen;

    [SerializeField] private GameObject m_patientHolder = default;
    [SerializeField] private float m_tweenAnimationDuration = 1.5f;

    private bool m_isDayOver = false;

    private Vector3 m_tweenStartPosition = new Vector3(-3.0f, 0.0f, 0.0f);
    private Vector3 m_tweenCenteredPosition = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector3 m_tweenEndPosition = new Vector3(3.0f, 0.0f, 0.0f);

    private void Awake()
    {
        GenerateNextPatient();
    }

    private void OnEnable()
    {
        Draggable.OnDraggableOnPatient += OnPlayerAction;
        DayCycle.OnTimerValueChanged += OnTimerValueChanged;
    }

    private void OnDisable()
    {
        Draggable.OnDraggableOnPatient -= OnPlayerAction;
        DayCycle.OnTimerValueChanged -= OnTimerValueChanged;
    }

    private void OnPlayerAction(Draggable obj)
    {
        OnPatientSeen?.Invoke();

        m_patientHolder.transform.DOMove(m_tweenEndPosition, m_tweenAnimationDuration).OnComplete(GenerateNextPatient);
    }

    private void GenerateNextPatient()
    {
        if (!m_isDayOver)
        {
            m_patientHolder.transform.position = m_tweenStartPosition;
            m_patientHolder.transform.DOMove(m_tweenCenteredPosition, m_tweenAnimationDuration);

            // Generate Patient
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