using TMPro;
using UnityEngine;

public class TopicRanking : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_topicScoreText = default;

    public void SetData(string title, int score)
    {
        m_topicScoreText.text = title + ": " + score.ToString();
    }
}