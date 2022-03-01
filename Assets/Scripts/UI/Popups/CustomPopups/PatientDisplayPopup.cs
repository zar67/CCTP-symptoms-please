using SymptomsPlease.UI.Popups;
using TMPro;
using UnityEngine;

public class PatientDisplayPopup : Popup
{
    [Header("Avatar Display")]
    [SerializeField] private AvatarDisplay m_avatarDisplay = default;

    [Header("Data References")]
    [SerializeField] private TextMeshProUGUI m_nameText = default;
    [SerializeField] private TextMeshProUGUI m_appointmentsText = default;
    [SerializeField] private TextMeshProUGUI m_previousActionsText = default;
    [SerializeField] private SymptomBubble m_symptomsBubblePrefab = default;
    [SerializeField] private Transform m_symptomsParent = default;

    private void Start()
    {
        PatientManager.OnNextPatient += OnNextPatient;
    }

    public override void OnOpenBegin()
    {
        base.OnOpenBegin();
        UpdateDisplay(PatientManager.CurrentPatient);
    }

    private void OnNextPatient(PatientData data)
    {
        UpdateDisplay(data);
    }

    private void UpdateDisplay(PatientData data)
    {
        m_avatarDisplay.UpdateSprites(data.AvatarData);
        m_nameText.text = data.Name;
        m_appointmentsText.text = data.PlayerStrikes.ToString();

        m_previousActionsText.text = "";
        foreach (ActionType action in data.PreviousActions)
        {
            m_previousActionsText.text += action.ToString() + ", ";
        }

        DisplayKnownSymptoms(data);
    }

    private void DisplayKnownSymptoms(PatientData data)
    {
        foreach (Transform child in m_symptomsParent)
        {
            Destroy(child.gameObject);
        }

        foreach (int symptom in data.SymptomsShown)
        {
            SymptomBubble newBubble = Instantiate(m_symptomsBubblePrefab, m_symptomsParent);
            newBubble.SetText(data.AfflictionData.GetSymptomAtIndex(symptom).Description);
        }
    }
}