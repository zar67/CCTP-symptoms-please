using SymptomsPlease.SaveSystem;
using SymptomsPlease.UI.Panels;
using UnityEngine;
using UnityEngine.UI;

public class OnlinePanel : Panel
{
    [SerializeField] private Button m_optInButton = default;
    [SerializeField] private Button m_optOutButton = default;

    [SerializeField] private GameObject m_optInContent = default;
    [SerializeField] private GameObject m_onlineContent = default;

    public override void OnOpen()
    {
        base.OnOpen();

        m_optInContent.SetActive(!GameData.EnabledOnline);
        m_onlineContent.SetActive(GameData.EnabledOnline);
    }

    protected override void Awake()
    {
        base.Awake();

        m_optInButton.onClick.AddListener(OnOptInClicked);
        m_optOutButton.onClick.AddListener(OnOptOutClicked);
    }

    private void OnOptInClicked()
    {
        GameData.EnabledOnline = true;
        SaveSystem.Save();

        m_optInContent.SetActive(!GameData.EnabledOnline);
        m_onlineContent.SetActive(GameData.EnabledOnline);
    }

    private void OnOptOutClicked()
    {
        GameData.EnabledOnline = false;
        SaveSystem.Save();

        m_optInContent.SetActive(!GameData.EnabledOnline);
        m_onlineContent.SetActive(GameData.EnabledOnline);
    }
}