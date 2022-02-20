using SymptomsPlease.UI.Panels;
using SymptomsPlease.UI.Popups;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayStartPanel : Panel
{
    [Header("Day Start Panel References")]
    [SerializeField] private TextMeshProUGUI m_dayNumberText = default;

    [Header("Popup References")]
    [SerializeField] private PopupData m_popupsData = default;
    [SerializeField] private string m_stiInformationPopupName = "";

    [Header("STI Information References")]
    [SerializeField] private AllAfflictionDatas m_afflictionDatas = default;
    [SerializeField] private STIInfoButton m_infoButtonPrefab = default;
    [SerializeField] private Transform m_stiInformationHolder = default;

    private STIInformationPopup m_stiInformationPopup = default;

    private void Start()
    {
        m_stiInformationPopup = (STIInformationPopup)m_popupsData.GetPopup(m_stiInformationPopupName);
    }

    public override void OnOpen()
    {
        base.OnOpen();

        m_dayNumberText.text = GameData.DayNumber.ToString();

        foreach (Transform child in m_stiInformationHolder)
        {
            Destroy(child.gameObject);
        }

        foreach (KeyValuePair<Topic, ModificationsManager.ModificationInstanceData> topic in ModificationsManager.UnhandledTopics)
        {
            foreach (AfflictionData affliction in m_afflictionDatas.GetAfflictionsWithTopic(topic.Key))
            {
                STIInfoButton display = Instantiate(m_infoButtonPrefab, m_stiInformationHolder);
                display.SelectButton.onClick.AddListener(() => m_stiInformationPopup.UpdateDisplay(affliction));
                display.SetNameText(affliction.DisplayName);
            }
        }
    }

    public override void OnClose()
    {
        base.OnClose();
        ModificationsManager.ClearNewTopics();
    }
}