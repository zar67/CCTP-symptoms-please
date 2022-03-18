using TMPro;
using UnityEngine;

public class TopicRanking : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_topicTitleText = default;
    [SerializeField] private TextMeshProUGUI m_topicScoreText = default;

    public void SetData(string title, int score)
    {
        m_topicTitleText.text = title + ":";
        m_topicScoreText.text = score.ToString();
    }
}