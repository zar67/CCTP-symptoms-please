using SymptomsPlease.UI.Popups;
using System.Collections.Generic;
using UnityEngine;

public class AfflictionsPopup : Popup
{
    [Header("Popup References")]
    [SerializeField] private string m_stiInformationPopupName = "";

    [SerializeField] private AllAfflictionDatas m_afflictionDatas = default;
    [SerializeField] private STIInfoButton m_infoButtonPrefab = default;
    [SerializeField] private Transform m_stiInformationHolder = default;

    private STIInformationPopup m_stiInformationPopup = default;

    private void Start()
    {
        m_stiInformationPopup = (STIInformationPopup)m_popupData.GetPopup(m_stiInformationPopupName);
    }

    private void OnEnable()
    {
        foreach (Transform child in m_stiInformationHolder)
        {
            Destroy(child.gameObject);
        }

        foreach (KeyValuePair<Topic, ModificationsManager.ModificationInstanceData> topic in ModificationsManager.ActiveTopics)
        {
            foreach (AfflictionData affliction in m_afflictionDatas.GetAfflictionsWithTopic(topic.Key))
            {
                STIInfoButton display = Instantiate(m_infoButtonPrefab, m_stiInformationHolder);
                display.SelectButton.onClick.AddListener(() => m_stiInformationPopup.UpdateDisplay(affliction));
                display.SetNameText(affliction.DisplayName);
            }
        }
    }
}