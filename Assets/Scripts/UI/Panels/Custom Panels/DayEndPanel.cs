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
    [SerializeField] private TextMeshProUGUI m_patientsStrikedOutText = default;
    [SerializeField] private TextMeshProUGUI m_successRateText = default;
    [SerializeField] private TextMeshProUGUI m_scoreText = default;

    [Header("Modifications References")]
    [SerializeField] private ModificationsData m_modificationData = default;

    public override void OnOpen()
    {
        base.OnOpen();

        m_dayNumberText.text = GameData.DayNumber.ToString();

        m_patientsSeenText.text = PatientManager.PatientSeenInDay.ToString();
        m_patientsHelpedText.text = PatientManager.PatientsHelpedInDay.ToString();
        m_patientsStrikedOutText.text = PatientManager.PatientsStrikedOutInDay.ToString();

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

        if (m_modificationData.DayHasModifcationActivations(GameData.DayNumber))
        {
            foreach (ModificationsData.DayActivationData mod in m_modificationData.GetActivationsForDay(GameData.DayNumber))
            {
                ModificationsManager.ActivateTopic(mod.Topic, mod.Description);
            }
        }

        if (m_modificationData.DayHasModifcationDeactivations(GameData.DayNumber))
        {
            foreach (Topic mod in m_modificationData.GetDeactivationsForDay(GameData.DayNumber))
            {
                ModificationsManager.DeactivateTopic(mod);
            }
        }

        SaveSystem.Save();
    }
}