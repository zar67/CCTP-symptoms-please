using SymptomsPlease.SaveSystem;
using SymptomsPlease.UI.Panels;
using SymptomsPlease.UI.Popups;
using UnityEngine;

public class MainMenuPanel : Panel
{
    [Header("FTUE Disclaimer Popup")]
    [SerializeField] private PopupData m_popupData = default;
    [SerializeField] private string m_ftueDisclaimerPopup = "popup_ftue_disclaimer";

    public override void OnOpen()
    {
        base.OnOpen();

        if (!FTUEManager.IsFTUETypeHandled(EFTUEType.DISCLAIMER_POPUP))
        {
            m_popupData.OpenPopup(m_ftueDisclaimerPopup);
            FTUEManager.HandleFTUEType(EFTUEType.DISCLAIMER_POPUP);
            SaveSystem.Save();
        }
    }
}