using SymptomsPlease.UI.Popups;
using System.Collections.Generic;
using UnityEngine;

public class AdviceBookPopup : Popup
{
    [Header("Advice Popup Variables")]
    [SerializeField] private Transform m_adviceHolder = default;
    [SerializeField] private AdviceObject m_adviceObject = default;

    private List<string> m_usedAdviceSlips = new List<string>();

    private void OnEnable()
    {
        PatientManager.OnNextPatient += OnNextPatient;
    }

    private void OnDisable()
    {
        PatientManager.OnNextPatient -= OnNextPatient;
    }

    private void OnNextPatient(PatientData patient)
    {
        m_usedAdviceSlips = new List<string>();
    }

    public override void OnOpenBegin()
    {
        PatientData currentPatient = PatientManager.CurrentPatient;

        foreach (Transform child in m_adviceHolder)
        {
            Destroy(child.gameObject);
        }

        foreach (AfflictionData.AdviceEffectivnessMap advice in currentPatient.AfflictionData.Advice)
        {
            if (!m_usedAdviceSlips.Contains(advice.Advice))
            {
                AdviceObject newAdvice = Instantiate(m_adviceObject, m_adviceHolder);
                newAdvice.SelectButton.onClick.AddListener(() =>
                {
                    PatientManager.HandleAdvice(advice.Advice);
                    m_popupData.ClosePopup(m_popupType);
                    m_usedAdviceSlips.Add(advice.Advice);
                });
                newAdvice.UpdateText(advice.Advice);
            }
        }
    }
}