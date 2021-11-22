using SymptomsPlease.UI.Panels;
using TMPro;
using UnityEngine;

public class DayEndPanel : Panel
{
    [Header("Day End Panel References")]
    [SerializeField] private TextMeshProUGUI m_patientsSeenText = default;
    [SerializeField] private TextMeshProUGUI m_patientsHelpedText = default;
    [SerializeField] private TextMeshProUGUI m_successRateText = default;
    [SerializeField] private TextMeshProUGUI m_scoreText = default;

    protected override void OnEnable()
    {
        base.OnEnable();

        m_patientsSeenText.text = PatientManager.PatientSeenInDay.ToString();
        m_patientsHelpedText.text = PatientManager.PatientsHelpedInDay.ToString();

        if (PatientManager.PatientSeenInDay > 0)
        {
            m_successRateText.text = ((int)((float)PatientManager.PatientsHelpedInDay / (float)PatientManager.PatientSeenInDay * 100)).ToString() + "%";
        }
        else
        {
            m_successRateText.text = "0%";
        }

        m_scoreText.text = DayCycle.Score.ToString();
    }
}