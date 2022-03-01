using DG.Tweening;
using SymptomsPlease.UI.Popups;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymptomsDisplayManager : MonoBehaviour
{
    [SerializeField] private SymptomBubble[] m_symptomBubbles = { };
    [SerializeField] private float m_fadeDuration = 0.5f;

    [Header("FTUE")]
    [SerializeField] private PopupData m_popupData = default;
    [SerializeField] private string m_symptomsFTUEPopup = "popup_ftue_symptoms";

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

        if (!FTUEManager.SeenSymptomsFTUE && PatientManager.PatientSeenInDay == 0)
        {
            StartCoroutine(ShowSymptomsFTUE());
            FTUEManager.SeenSymptomsFTUE = true;
        }
    }

    private IEnumerator ShowSymptomsFTUE()
    {
        yield return new WaitForSeconds(m_fadeDuration);

        m_popupData.OpenPopup(m_symptomsFTUEPopup);
    }
}