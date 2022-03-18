using SymptomsPlease.UI.Panels;
using SymptomsPlease.UI.Popups;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrainingPanel : Panel
{
    [Header("Popup References")]
    [SerializeField] private PopupData m_popupData = default;

    [Header("Ranking References")]
    [SerializeField] private Transform m_rankingsParent = default;
    [SerializeField] private TopicRanking m_topicRankingPrefab = default;

    public override void OnOpen()
    {
        base.OnOpen();

        m_popupData.GetPopup("popup_training_quiz").OnCloseEvent += OnQuizClosed;
    }

    private void OnQuizClosed()
    {
        UpdateDisplay();
    }

    private void OnEnable()
    {
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        foreach (Transform child in m_rankingsParent)
        {
            Destroy(child.gameObject);
        }

        foreach (KeyValuePair<Topic, int> topic in TrainingManager.GetRankedTopics())
        {
            TopicRanking newRanking = Instantiate(m_topicRankingPrefab, m_rankingsParent);
            newRanking.SetData(topic.Key.ToString(), topic.Value);
        }
    }
}