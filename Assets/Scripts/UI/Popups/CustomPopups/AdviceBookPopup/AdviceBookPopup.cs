using SymptomsPlease.UI.Popups;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AdviceBookPopup : Popup
{
    public static event Action<string> OnAdviceGiven;

    [Header("Advice Popup Variables")]
    [SerializeField] private Transform m_adviceHolder = default;
    [SerializeField] private AdviceObject m_adviceObject = default;

    private List<string> m_usedAdviceSlips = new List<string>();

    protected override void Awake()
    {
        base.Awake();
        PatientManager.OnNextPatient += OnNextPatient;
    }

    private void OnNextPatient(PatientData patient)
    {
        m_usedAdviceSlips = new List<string>();
        UpdateDisplay(patient);
    }

    public override void OnOpenBegin()
    {
        base.OnOpenBegin();
        UpdateDisplay(PatientManager.CurrentPatient);
    }

    private void UpdateDisplay(PatientData data)
    {
        foreach (Transform child in m_adviceHolder)
        {
            Destroy(child.gameObject);
        }

        foreach (AfflictionData.AdviceEffectivnessMap advice in data.AfflictionData.Advice)
        {
            if (!m_usedAdviceSlips.Contains(advice.Advice))
            {
                AdviceObject newAdvice = Instantiate(m_adviceObject, m_adviceHolder);
                newAdvice.SelectButton.onClick.AddListener(() =>
                {
                    OnAdviceGiven?.Invoke(advice.Advice);
                    m_popupData.ClosePopup(m_popupType);
                    m_usedAdviceSlips.Add(advice.Advice);
                });
                newAdvice.UpdateText(advice.Advice);
            }
        }
    }
}