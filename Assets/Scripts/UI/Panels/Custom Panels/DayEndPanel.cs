using SymptomsPlease.SaveSystem;
using SymptomsPlease.UI.Panels;
using TMPro;
using UnityEngine;

public class DayEndPanel : Panel
{
    [Header("Day End Panel References")]
    [SerializeField] private TextMeshProUGUI m_dayNumberText = default;
    [SerializeField] private TextMeshProUGUI m_patientsSeenText = default;
    [SerializeField] private TextMeshProUGUI m_patientsHelpedText = default;
    [SerializeField] private TextMeshProUGUI m_successRateText = default;
    [SerializeField] private TextMeshProUGUI m_scoreText = default;

    public override void OnOpen()
    {
        base.OnOpen();

        m_dayNumberText.text = GameData.DayNumber.ToString();

        m_patientsSeenText.text = PatientManager.PatientSeenInDay.ToString();
        m_patientsHelpedText.text = PatientManager.PatientsHelpedInDay.ToString();

        if (PatientManager.PatientSeenInDay > 0)
        {
            int successRate = (int)(PatientManager.PatientsHelpedInDay / (float)PatientManager.PatientSeenInDay * 100);
            m_successRateText.text = successRate.ToString() + "%";
        }
        else
        {
            m_successRateText.text = "0%";
        }

        m_scoreText.text = DayCycle.Score.ToString();

        GameData.DayNumber++;
        GameData.TotalPatientsHelped += PatientManager.PatientsHelpedInDay;
        GameData.TotalPatientsSeen += PatientManager.PatientSeenInDay;
        GameData.TotalScore += DayCycle.Score;

        SaveSystem.Save();
    }
}