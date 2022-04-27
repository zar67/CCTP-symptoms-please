using SymptomsPlease.UI.Panels;
using SymptomsPlease.UI.Popups;
using System;
using TMPro;
using UnityEngine;

public class ProfilePanel : Panel
{
    [Header("Popup References")]
    [SerializeField] private PopupData m_popupData = default;

    [Header("Display References")]
    [SerializeField] private TextMeshProUGUI m_nameText = default;

    [SerializeField] private TextMeshProUGUI m_timePlayedText = default;
    [SerializeField] private TextMeshProUGUI m_patientsHelpedText = default;
    [SerializeField] private TextMeshProUGUI m_successRateText = default;
    [SerializeField] private AvatarDisplay m_avatarDisplay = default;

    public override void OnOpen()
    {
        base.OnOpen();
        m_popupData.GetPopup("popup_name_change").OnCloseEvent += UpdateName;
        m_popupData.GetPopup("popup_customise_avatar").OnCloseEvent += UpdateAvatar;

        UpdateName();

        var totalTimePlayed = TimeSpan.FromSeconds(GameData.TotalTimePlayed);
        m_timePlayedText.text = "Time Played: " + totalTimePlayed.ToString(@"hh\:mm");
        m_patientsHelpedText.text = "Patients Helped: " + GameData.TotalPatientsHelped.ToString();

        int successRate = 0;
        if (GameData.TotalPatientsSeen != 0)
        {
            successRate = (int)(GameData.TotalPatientsHelped / (float)GameData.TotalPatientsSeen * 100);
        }
        m_successRateText.text = "Success Rate: " + successRate.ToString() + "%";
    }

    public override void OnClose()
    {
        base.OnClose();
        m_popupData.GetPopup("popup_name_change").OnCloseEvent -= UpdateName;
        m_popupData.GetPopup("popup_customise_avatar").OnCloseEvent -= UpdateAvatar;
    }

    private void UpdateName()
    {
        m_nameText.text = GameData.PlayerName;
    }

    private void UpdateAvatar()
    {
        m_avatarDisplay.UpdateSprites(GameData.AvatarData);
    }
}