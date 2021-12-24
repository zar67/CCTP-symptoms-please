using SymptomsPlease.UI.Panels;
using TMPro;
using UnityEngine;

public class DayStartPanel : Panel
{
    [Header("Day Start Panel References")]
    [SerializeField] private TextMeshProUGUI m_dayNumberText = default;

    public override void OnOpen()
    {
        base.OnOpen();

        m_dayNumberText.text = GameData.DayNumber.ToString();
    }
}