using SymptomsPlease.UI.Panels;
using TMPro;
using UnityEngine;

public class DayEndPanel : Panel
{
    [Header("Day End Panel References")]
    [SerializeField] private TextMeshProUGUI m_patientsSeenText = default;
    [SerializeField] private TextMeshProUGUI m_patientsHelpedText = default;
    [SerializeField] private TextMeshProUGUI m_successRateText = default;

    protected override void OnEnable()
    {
        base.OnEnable();

        m_patientsSeenText.text = DayCycle.PatientSeenInDay.ToString();
        m_patientsHelpedText.text = DayCycle.PatientsHelpedInDay.ToString();

        if (DayCycle.PatientSeenInDay > 0)
        {
            m_successRateText.text = ((int)((float)DayCycle.PatientsHelpedInDay / (float)DayCycle.PatientSeenInDay * 100)).ToString() + "%";
        }
        else
        {
            m_successRateText.text = "0%";
        }
    }
}