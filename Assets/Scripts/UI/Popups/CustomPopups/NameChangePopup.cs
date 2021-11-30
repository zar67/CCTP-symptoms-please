using SymptomsPlease.SaveSystem;
using SymptomsPlease.UI.Popups;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NameChangePopup : Popup
{
    [Header("Name Change References")]
    [SerializeField] private TextMeshProUGUI m_nameText = default;
    [SerializeField] private Button m_generateNameButton = default;
    [SerializeField] private Button m_saveNameButton = default;

    [Header("Valid Names")]
    [SerializeField] private string[] m_validNamesList = { };

    private void OnEnable()
    {
        m_generateNameButton.onClick.AddListener(OnGenerateName);
        m_saveNameButton.onClick.AddListener(OnSaveName);

        m_nameText.text = GameData.PlayerName;
    }

    private void OnDisable()
    {
        m_generateNameButton.onClick.RemoveListener(OnGenerateName);
        m_saveNameButton.onClick.RemoveListener(OnSaveName);
    }

    private void OnGenerateName()
    {
        int randomIndex = Random.Range(0, m_validNamesList.Length);
        m_nameText.text = m_validNamesList[randomIndex];
    }

    private void OnSaveName()
    {
        GameData.PlayerName = m_nameText.text;
        m_popupData.ClosePopup(m_popupType);
        SaveSystem.Save();
    }
}