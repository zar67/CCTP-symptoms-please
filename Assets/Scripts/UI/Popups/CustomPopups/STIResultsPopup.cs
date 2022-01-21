using SymptomsPlease.UI.Popups;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class STIResultsPopup : Popup
{
    [Serializable]
    private struct AfflictionCheckmarkMap
    {
        public AfflictionData AfflictionData;
        public Image[] CheckmarkImages;
    }

    [SerializeField] private TextMeshProUGUI m_patientNameText = default;

    [Header("Test Type Holders")]
    [SerializeField] private GameObject m_bloodTestHolder = default;
    [SerializeField] private GameObject m_urineTestHolder = default;
    [SerializeField] private GameObject m_swabTestHolder = default;

    [Header("Affliction Map References")]
    [SerializeField] private AfflictionCheckmarkMap[] m_afflictionCheckmarkMap;

    [Header("Sprite References")]
    [SerializeField] private Sprite m_positiveSprite = default;
    [SerializeField] private Sprite m_negativeSprite = default;

    public override void OnOpenBegin()
    {
        base.OnOpenBegin();

        m_bloodTestHolder.SetActive(PatientManager.CurrentAction == ActionType.GIVE_BLOOD_TEST_KIT);
        m_urineTestHolder.SetActive(PatientManager.CurrentAction == ActionType.GIVE_URINE_TEST_KIT);
        m_swabTestHolder.SetActive(PatientManager.CurrentAction == ActionType.GIVE_SWAB_TEST_KIT);

        string currentAffliction = PatientManager.CurrentPatient.AfflictionData.DisplayName;
        foreach (AfflictionCheckmarkMap affliction in m_afflictionCheckmarkMap)
        {
            foreach (Image image in affliction.CheckmarkImages)
            {
                image.sprite = affliction.AfflictionData.DisplayName == currentAffliction ? m_positiveSprite : m_negativeSprite;
            }
        }

        m_patientNameText.text = PatientManager.CurrentPatient.Name;
    }
}