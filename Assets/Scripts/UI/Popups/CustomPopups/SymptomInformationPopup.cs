using SymptomsPlease.UI.Popups;
using TMPro;
using UnityEngine;

public class SymptomInformationPopup : Popup
{
    [Header("Display References")]
    [SerializeField] private TextMeshProUGUI m_descriptionText = default;

    public override void Initialise()
    {
        base.Initialise();
        SymptomBubble.OnSymptomSelected += OnSymptomSelected;
    }

    private void OnSymptomSelected(SymptomsData data)
    {
        m_descriptionText.text = data.Description;
    }
}