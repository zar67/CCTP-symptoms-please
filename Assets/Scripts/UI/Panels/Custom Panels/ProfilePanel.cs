using SymptomsPlease.UI.Panels;
using System;
using TMPro;
using UnityEngine;

public class ProfilePanel : Panel
{
    [SerializeField] private TextMeshProUGUI m_nameText = default;

    [SerializeField] private TextMeshProUGUI m_timePlayedText = default;
    [SerializeField] private TextMeshProUGUI m_patientsHelpedText = default;
    [SerializeField] private TextMeshProUGUI m_successRateText = default;

    public override void OnOpen()
    {
        base.OnOpen();

        m_nameText.text = GameData.PlayerName;

        var totalTimePlayed = TimeSpan.FromSeconds(GameData.TotalTimePlayed);
        m_timePlayedText.text = totalTimePlayed.ToString(@"hh\:mm");
        m_patientsHelpedText.text = GameData.TotalPatientsHelped.ToString();

        int successRate = 0;
        if (GameData.TotalPatientsSeen != 0)
        {
            successRate = (int)((float)GameData.TotalPatientsHelped / (float)GameData.TotalPatientsSeen * 100);
        }
        m_successRateText.text = successRate.ToString();
    }
}