using SymptomsPlease.UI.Popups;
using TMPro;
using UnityEngine;

public class STIInformationPopup : Popup
{
    [Header("Data References")]
    [SerializeField] private ActionsData m_actionsData = default;

    [Header("Information References")]
    [SerializeField] private TextMeshProUGUI m_nameText = default;
    [SerializeField] private TextMeshProUGUI m_descriptionText = default;
    [SerializeField] private Transform m_symptomsHolder = default;
    [SerializeField] private Transform m_treatmentsHolder = default;
    [SerializeField] private InformationHolder m_informationHolderPrefab = default;

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
            InformationHolder newbubble = Instantiate(m_informationHolderPrefab, m_symptomsHolder);
            newbubble.SetText(affliction.GetSymptomAtIndex(i).DisplayName);
        }

        RectTransform symptomsRect = m_symptomsHolder.GetComponent<RectTransform>();
        symptomsRect.sizeDelta = new Vector2(symptomsRect.sizeDelta.x, 150 * (affliction.SymptomsCount / 3));

        foreach (Transform child in m_treatmentsHolder)
        {
            Destroy(child.gameObject);
        }

        int numTreatments = 0;
        foreach(ActionType type in affliction.GetTreatments())
        {
            InformationHolder newbubble = Instantiate(m_informationHolderPrefab, m_treatmentsHolder);
            newbubble.SetText(m_actionsData.GetInfoForAction(type).DisplayName);
            numTreatments++;
        }

        RectTransform treatmentsRect = m_treatmentsHolder.GetComponent<RectTransform>();
        treatmentsRect.sizeDelta = new Vector2(treatmentsRect.sizeDelta.x, 150 * (numTreatments / 3));

        foreach (string advice in affliction.GetAdviceTreatment())
        {
            InformationHolder newbubble = Instantiate(m_informationHolderPrefab, m_treatmentsHolder);
            newbubble.SetText("Advice: " + advice.ToString());
        }
    }
}