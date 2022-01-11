using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class SymptomsDisplayManager : MonoBehaviour
{
    [SerializeField] private SymptomBubble[] m_symptomBubbles = { };
    [SerializeField] private float m_delayBetweenBubbles = 0.9f;

    private List<Tween> m_fadeInTweens = new List<Tween>();

    private void Awake()
    {
        foreach (SymptomBubble bubble in m_symptomBubbles)
        {
            bubble.CanvasGroup.alpha = 0;
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
            bubble.CanvasGroup.DOFade(0, 0.5f);
        }
    }

    private void OnNextPatient(PatientData patient)
    {
        int numWrittenSymptoms = patient.AfflictionData.WrittenSymptomsCount;
        int numIconSymptoms = patient.AfflictionData.IconSymptomsCount;

        if (patient.AfflictionData.WrittenSymptomsCount + patient.AfflictionData.IconSymptomsCount > m_symptomBubbles.Length)
        {
            int writtenSymptomsMax = Mathf.Min(patient.AfflictionData.WrittenSymptomsCount, m_symptomBubbles.Length);

            numWrittenSymptoms = Random.Range(0, writtenSymptomsMax + 1);
            numIconSymptoms = Mathf.Min(m_symptomBubbles.Length - numWrittenSymptoms, patient.AfflictionData.IconSymptomsCount);

            for (int i = 0; i < m_symptomBubbles.Length - (numWrittenSymptoms + numIconSymptoms); i++)
            {
                if (numWrittenSymptoms == patient.AfflictionData.WrittenSymptomsCount &&
                    numIconSymptoms == patient.AfflictionData.IconSymptomsCount)
                {
                    break;
                }

                if (numWrittenSymptoms < patient.AfflictionData.WrittenSymptomsCount)
                {
                    numWrittenSymptoms++;
                }

                if (numIconSymptoms < patient.AfflictionData.IconSymptomsCount)
                {
                    numIconSymptoms++;
                }
            }
        }

        int count = 0;
        foreach (string writtenSymptom in patient.AfflictionData.GetRandomStringSymptom(numWrittenSymptoms))
        {
            m_symptomBubbles[count].SetWrittenSymptomText(writtenSymptom);
            count++;
        }

        foreach (Sprite iconSymptom in patient.AfflictionData.GetRandomIconSymptom(numIconSymptoms))
        {
            m_symptomBubbles[count].SetIconSymptomSprite(iconSymptom);
            count++;
        }

        for (int i = 0; i < m_symptomBubbles.Length; i++)
        {
            if (i < count)
            {
                m_symptomBubbles[i].gameObject.SetActive(true);
                m_symptomBubbles[i].CanvasGroup.alpha = 0;

                Tween newTween = m_symptomBubbles[i].CanvasGroup.DOFade(1.0f, 1.0f);
                newTween.SetDelay((i * m_delayBetweenBubbles) + m_delayBetweenBubbles);
                m_fadeInTweens.Add(newTween);
            }
            else
            {
                m_symptomBubbles[i].gameObject.SetActive(false);
            }
        }
    }
}