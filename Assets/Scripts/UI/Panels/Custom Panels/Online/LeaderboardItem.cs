using TMPro;
using UnityEngine;

public class LeaderboardItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_positionText = default;
    [SerializeField] private TextMeshProUGUI m_nameText = default;
    [SerializeField] private TextMeshProUGUI m_scoreText = default;

    public void SetPostitionText(int position)
    {
        m_positionText.text = CreateOrdinalString(position);
    }

    public void SetNameText(string name)
    {
        m_nameText.text = name;
    }

    public void SetScoreText(int score)
    {
        m_scoreText.text = score.ToString();
    }

    private string CreateOrdinalString(int position)
    {
        if (position <= 0)
        {
            return position.ToString();
        }

        switch (position % 100)
        {
            case 11:
            case 12:
            case 13:
            {
                return position + "th";
            }
        }

        return (position % 10) switch
        {
            1 => position + "st",
            2 => position + "nd",
            3 => position + "rd",
            _ => position + "th",
        };
    }
}