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

    public override void OnOpenBegin()
    {
        base.OnOpenBegin();

        m_avatarDisplay.UpdateSprites(PatientManager.CurrentPatient.AvatarData);
        m_nameText.text = PatientManager.CurrentPatient.Name;
        m_appointmentsText.text = PatientManager.CurrentPatient.PlayerStrikes.ToString();

        m_previousActionsText.text = "";
        foreach (ActionType action in PatientManager.CurrentPatient.PreviousActions)
        {
            m_previousActionsText.text += action.ToString() + ", ";
        }

        DisplayKnownSymptoms(PatientManager.CurrentPatient);
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