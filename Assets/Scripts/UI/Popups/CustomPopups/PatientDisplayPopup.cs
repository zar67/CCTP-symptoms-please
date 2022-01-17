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

        DisplayKnownSymptoms(data);
    }

    private void DisplayKnownSymptoms(PatientData data)
    {
        foreach (Transform child in m_symptomsParent)
        {
            Destroy(child.gameObject);
        }

        var workingWrittenSymptomsShown = new List<int>(data.WrittenSymptomsShown);
        var workingIconSymptomsShown = new List<int>(data.IconSymptomsShown);

        int totalSymptoms = workingWrittenSymptomsShown.Count + workingIconSymptomsShown.Count;

        for (int i = 0; i < totalSymptoms; i++)
        {
            if (workingWrittenSymptomsShown.Count == 0 && workingIconSymptomsShown.Count == 0)
            {
                return;
            }
            else if (workingWrittenSymptomsShown.Count == 0)
            {
                int randomIndex = Random.Range(0, workingIconSymptomsShown.Count);
                workingIconSymptomsShown.RemoveAt(randomIndex);

                SymptomBubble newBubble = Instantiate(m_symptomsBubblePrefab, m_symptomsParent);
                newBubble.SetIconSymptomSprite(data.AfflictionData.GetIconSymptomAtIndex(randomIndex));
            }
            else if (workingIconSymptomsShown.Count == 0)
            {
                int randomIndex = Random.Range(0, workingWrittenSymptomsShown.Count);
                workingWrittenSymptomsShown.RemoveAt(randomIndex);

                SymptomBubble newBubble = Instantiate(m_symptomsBubblePrefab, m_symptomsParent);
                newBubble.SetWrittenSymptomText(data.AfflictionData.GetWrittenSymptomAtIndex(randomIndex));
            }
            else
            {
                int randomChance = Random.Range(0, 101);
                if (randomChance < 50)
                {
                    int randomIndex = Random.Range(0, workingWrittenSymptomsShown.Count);
                    workingWrittenSymptomsShown.RemoveAt(randomIndex);

                    SymptomBubble newBubble = Instantiate(m_symptomsBubblePrefab, m_symptomsParent);
                    newBubble.SetWrittenSymptomText(data.AfflictionData.GetWrittenSymptomAtIndex(randomIndex));
                }
                else
                {
                    int randomIndex = Random.Range(0, workingIconSymptomsShown.Count);
                    workingIconSymptomsShown.RemoveAt(randomIndex);

                    SymptomBubble newBubble = Instantiate(m_symptomsBubblePrefab, m_symptomsParent);
                    newBubble.SetIconSymptomSprite(data.AfflictionData.GetIconSymptomAtIndex(randomIndex));
                }
            }
        }
    }
}