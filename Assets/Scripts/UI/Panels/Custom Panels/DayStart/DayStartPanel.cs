using SymptomsPlease.UI.Panels;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayStartPanel : Panel
{
    public static event Action<AfflictionData> OnAfflictionSelected;

    [Header("Day Start Panel References")]
    [SerializeField] private TextMeshProUGUI m_dayNumberText = default;

    [Header("STI Information References")]
    [SerializeField] private AllAfflictionDatas m_afflictionDatas = default;
    [SerializeField] private STIInfoButton m_infoButtonPrefab = default;
    [SerializeField] private Transform m_stiInformationHolder = default;

    public override void OnOpen()
    {
        base.OnOpen();

        m_dayNumberText.text = GameData.DayNumber.ToString();

        foreach (Transform child in m_stiInformationHolder)
        {
            Destroy(child.gameObject);
        }

        foreach (KeyValuePair<Topic, ModificationsManager.ModificationInstanceData> topic in ModificationsManager.ActiveTopics)
        {
            foreach (AfflictionData affliction in m_afflictionDatas.GetAfflictionsWithTopic(topic.Key))
            {
                STIInfoButton display = Instantiate(m_infoButtonPrefab, m_stiInformationHolder);
                display.SelectButton.onClick.AddListener(() => OnAfflictionSelected?.Invoke(affliction));
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