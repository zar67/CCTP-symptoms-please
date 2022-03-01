using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class SymptomsDisplayManager : MonoBehaviour
{
    [SerializeField] private SymptomBubble[] m_symptomBubbles = { };
    [SerializeField] private float m_delayBetweenBubbles = 0.9f;
    [SerializeField] private float m_fadeDuration = 0.5f;

    private List<Tween> m_fadeInTweens = new List<Tween>();

    private void Awake()
    {
        DayCycle.OnDayStarted += OnDayStarted;
    }

    private void OnDayStarted()
    {
        foreach (SymptomBubble bubble in m_symptomBubbles)
        {
            bubble.CanvasGroup.DOFade(0, m_fadeDuration);
        }
    }

    private void OnEnable()
    {
        PatientManager.OnPatientSeen += OnPatientSeen;
        PatientManager.OnNextPatient += OnNextPatient;
    }

    private void OnDisable()
    {
        PatientManager.OnPatientSeen -= OnPatientSeen;
        PatientManager.OnNextPatient -= OnNextPatient;
    }

    private void OnPatientSeen(PatientManager.PatientSeenData data)
    {
        foreach (Tween tween in m_fadeInTweens)
        {
            tween.Kill();
        }
        m_fadeInTweens = new List<Tween>();

        foreach (SymptomBubble bubble in m_symptomBubbles)
        {
            bubble.CanvasGroup.DOFade(0, m_fadeDuration);
        }
    }

    private void OnNextPatient(PatientData patient)
    {
        int count = 0;

        foreach (SymptomsData symptom in patient.AfflictionData.GetRandomSymptoms(patient, Random.Range(1, m_symptomBubbles.Length)))
        {
            m_symptomBubbles[count].SetText(symptom.Description);
            count++;
        }

        for (int i = 0; i < m_symptomBubbles.Length; i++)
        {
            if (i < count)
            {
                m_symptomBubbles[i].gameObject.SetActive(true);
                m_symptomBubbles[i].CanvasGroup.alpha = 0;

                Tween newTween = m_symptomBubbles[i].CanvasGroup.DOFade(1.0f, m_fadeDuration);
                m_fadeInTweens.Add(newTween);
            }
            else
            {
                m_symptomBubbles[i].gameObject.SetActive(false);
            }
        }
    }
}