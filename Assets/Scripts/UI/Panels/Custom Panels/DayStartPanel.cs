using SymptomsPlease.UI.Panels;
using TMPro;
using UnityEngine;

public class DayStartPanel : Panel
{
    [Header("Day Start Panel References")]
    [SerializeField] private TextMeshProUGUI m_dayNumberText = default;

    protected override void OnEnable()
    {
        base.OnEnable();

        m_dayNumberText.text = GameData.DayNumber.ToString();
    }
}