using SymptomsPlease.UI.Popups;
using TMPro;
using UnityEngine;

public class STIInformationPopup : Popup
{
    [Header("Information References")]
    [SerializeField] private TextMeshProUGUI m_nameText = default;
    [SerializeField] private TextMeshProUGUI m_descriptionText = default;
    [SerializeField] private Transform m_symptomsHolder = default;
    [SerializeField] private Transform m_treatmentsHolder = default;
    [SerializeField] private SymptomBubble m_symptomBubblePrefab = default;

    public void UpdateDisplay(AfflictionData affliction)
    {
        m_nameText.text = affliction.DisplayName;
        m_descriptionText.text = affliction.Description;

        foreach (Transform child in m_symptomsHolder)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < affliction.SymptomsCount; i++)
        {
            SymptomBubble newbubble = Instantiate(m_symptomBubblePrefab, m_symptomsHolder);
            newbubble.SetText(affliction.GetSymptomAtIndex(i).Description);
        }

        foreach (Transform child in m_treatmentsHolder)
        {
            Destroy(child.gameObject);
        }

        foreach(ActionType type in affliction.GetTreatments())
        {
            SymptomBubble newbubble = Instantiate(m_symptomBubblePrefab, m_treatmentsHolder);
            newbubble.SetText(type.ToString());
        }

        foreach (string advice in affliction.GetAdviceTreatment())
        {
            SymptomBubble newbubble = Instantiate(m_symptomBubblePrefab, m_treatmentsHolder);
            newbubble.SetText("Advice: " + advice.ToString());
        }
    }
}