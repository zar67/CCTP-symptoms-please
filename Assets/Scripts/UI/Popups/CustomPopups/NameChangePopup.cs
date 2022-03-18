using SymptomsPlease.SaveSystem;
using SymptomsPlease.UI.Popups;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NameChangePopup : Popup
{
    [Header("Name Change References")]
    [SerializeField] private TextMeshProUGUI m_nameText = default;
    [SerializeField] private Button m_generateNameButton = default;
    [SerializeField] private Button m_saveNameButton = default;

    private string m_previouslyGeneratedName = "";

    private void OnEnable()
    {
        m_generateNameButton.onClick.AddListener(OnGenerateName);
        m_saveNameButton.onClick.AddListener(OnSaveName);

        m_nameText.text = GameData.PlayerName;
        m_previouslyGeneratedName = "";
    }

    private void OnDisable()
    {
        m_generateNameButton.onClick.RemoveListener(OnGenerateName);
        m_saveNameButton.onClick.RemoveListener(OnSaveName);
    }

    private void OnGenerateName()
    {
        AudioManager.Instance.Play(EAudioClipType.CLICK);

        List<string> availableNames = GameData.AvailablePlayerNames;
        int randomIndex = Random.Range(0, availableNames.Count);
        string newName = availableNames[randomIndex];

        if (m_previouslyGeneratedName != "")
        {
            FirebaseDatabaseManager.UnresevePlayerName(m_previouslyGeneratedName);
        }

        FirebaseDatabaseManager.ReservePlayerName(newName);
        m_previouslyGeneratedName = newName;

        m_nameText.text = newName;
    }

    private void OnSaveName()
    {
        AudioManager.Instance.Play(EAudioClipType.CLICK);

        if (m_previouslyGeneratedName != "")
        {
            FirebaseDatabaseManager.UnresevePlayerName(GameData.PlayerName);
            GameData.PlayerName = m_previouslyGeneratedName;
            SaveSystem.Save();
        }

        m_popupData.ClosePopup(m_popupType);
    }
}