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

    public override void OnOpen()
    {
        base.OnOpen();

        m_dayNumberText.text = GameData.DayNumber.ToString();
    }

    public override void OnClose()
    {
        base.OnClose();
        ModificationsManager.ClearNewTopics();
    }
}