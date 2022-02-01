using SymptomsPlease.UI.Popups;
using System.Collections.Generic;
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

    public override void Initialise()
    {
        base.Initialise();

        PatientDisplay.OnPatientSelected += OnPatientSelected;
    }

    private void OnPatientSelected(PatientData data)
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
            newBubble.SetSymptomData(data.AfflictionData.GetSymptomAtIndex(symptom));
        }
    }
}