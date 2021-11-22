using SymptomsPlease.UI.Panels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayEndPanel : Panel
{
    [Header("Day End Panel References")]
    [SerializeField] private TextMeshProUGUI m_dayNumberText = default;
    [SerializeField] private TextMeshProUGUI m_patientsSeenText = default;
    [SerializeField] private TextMeshProUGUI m_patientsHelpedText = default;
    [SerializeField] private TextMeshProUGUI m_successRateText = default;
    [SerializeField] private TextMeshProUGUI m_scoreText = default;
    [SerializeField] private Button m_continueButton = default;

    protected override void Awake()
    {
        base.Awake();

        m_continueButton.onClick.AddListener(OnContinue);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        m_dayNumberText.text = PlayerPrefs.GetInt("DaysCompleted").ToString();

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

    private void OnContinue()
    {
        PlayerPrefs.SetInt("DaysCompleted", PlayerPrefs.GetInt("DaysCompleted") + 1);
    }
}