using DG.Tweening;
using UnityEngine;

public class DisplaySymptomsManager : MonoBehaviour
{
    [SerializeField] private SymptomBubble[] m_symptomBubbles = { };

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

    private void OnPatientSeen(bool helped)
    {
        foreach (SymptomBubble bubble in m_symptomBubbles)
        {
            bubble.CanvasGroup.DOFade(0, 0.5f);
        }
    }

    private void OnNextPatient(PatientData patient)
    {
        int numWrittenSymptoms = patient.WrittenSymptomsCount;
        int numIconSymptoms = patient.IconSymptomsCount;

        if (patient.WrittenSymptomsCount + patient.IconSymptomsCount > m_symptomBubbles.Length)
        {
            int writtenSymptomsMax = Mathf.Min(patient.WrittenSymptomsCount, m_symptomBubbles.Length);

            numWrittenSymptoms = Random.Range(0, writtenSymptomsMax + 1);
            numIconSymptoms = Mathf.Min(m_symptomBubbles.Length - numWrittenSymptoms, patient.IconSymptomsCount);

            for (int i = 0; i < m_symptomBubbles.Length - (numWrittenSymptoms + numIconSymptoms); i++)
            {
                if (numWrittenSymptoms == patient.WrittenSymptomsCount &&
                    numIconSymptoms == patient.IconSymptomsCount)
                {
                    break;
                }

                if (numWrittenSymptoms < patient.WrittenSymptomsCount)
                {
                    numWrittenSymptoms++;
                }

                if (numIconSymptoms < patient.IconSymptomsCount)
                {
                    numIconSymptoms++;
                }
            }
        }

        int count = 0;
        foreach (string writtenSymptom in patient.GetRandomStringSymptom(numWrittenSymptoms))
        {
            m_symptomBubbles[count].SetWrittenSymptomText(writtenSymptom);
            count++;
        }

        foreach (Sprite iconSymptom in patient.GetRandomIconSymptom(numIconSymptoms))
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

                // Animate In One After The Other
                m_symptomBubbles[i].CanvasGroup.DOFade(1.0f, 1.0f);
            }
            else
            {
                m_symptomBubbles[i].gameObject.SetActive(false);
            }
        }
    }
}