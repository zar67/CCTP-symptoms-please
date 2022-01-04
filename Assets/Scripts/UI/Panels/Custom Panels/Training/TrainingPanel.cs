using SymptomsPlease.UI.Panels;
using SymptomsPlease.UI.Popups;
using TMPro;
using UnityEngine;

public class TrainingPanel : Panel
{
    [Header("Popup References")]
    [SerializeField] private PopupData m_popupData = default;

    [Header("Training Topics")]
    [SerializeField] private TextMeshProUGUI m_bestTopicText = default;
    [SerializeField] private TextMeshProUGUI m_worstTopicText = default;

    public override void OnOpen()
    {
        base.OnOpen();

        m_popupData.GetPopup("popup_training_quiz").OnCloseEvent += OnQuizClosed;
    }

    private void OnQuizClosed()
    {
        m_bestTopicText.text = TrainingManager.GetBestTopic().ToString();
        m_worstTopicText.text = TrainingManager.GetWorstTopic().ToString();
    }

    private void OnEnable()
    {
        m_bestTopicText.text = TrainingManager.GetBestTopic().ToString();
        m_worstTopicText.text = TrainingManager.GetWorstTopic().ToString();
    }
}