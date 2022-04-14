using SymptomsPlease.UI.Popups;
using System.Collections.Generic;
using UnityEngine;

public class SocialMediaModificationsDisplay : MonoBehaviour
{
    [SerializeField] private ModificationsData m_modificationsData = default;

    [SerializeField] private Transform m_scrollViewContent = default;
    [SerializeField] private ModificationDisplay m_displayPrefab = default;

    [Header("Popup References")]
    [SerializeField] private PopupData m_popupsData = default;
    [SerializeField] private string m_stiInformationPopupName = "";

    [Header("STI Information References")]
    [SerializeField] private AllAfflictionDatas m_afflictionDatas = default;
    [SerializeField] private STIInfoButton m_infoButtonPrefab = default;

    private STIInformationPopup m_stiInformationPopup = default;

    private void Start()
    {
        m_stiInformationPopup = (STIInformationPopup)m_popupsData.GetPopup(m_stiInformationPopupName);
    }


    private void OnEnable()
    {
        foreach (Transform child in m_scrollViewContent)
        {
            Destroy(child.gameObject);
        }

        if (ModificationsManager.NumActiveTopics == 0)
        {
            ModificationsManager.ActivateTopic(Topic.CHLAMYDIA, m_modificationsData.GetDefaultDescriptionForTopic(Topic.CHLAMYDIA));
            ModificationsManager.ActivateTopic(Topic.PUBLIC_LICE, m_modificationsData.GetDefaultDescriptionForTopic(Topic.PUBLIC_LICE));
        }

        foreach (KeyValuePair<Topic, ModificationsManager.ModificationInstanceData> topic in ModificationsManager.UnhandledTopics)
        {
            ModificationDisplay display = Instantiate(m_displayPrefab, m_scrollViewContent);
            display.SetText(topic.Value.Description);

            foreach (AfflictionData affliction in m_afflictionDatas.GetAfflictionsWithTopic(topic.Key))
            {
                STIInfoButton infoButton = Instantiate(m_infoButtonPrefab, display.InfoButtonHolder);
                infoButton.SelectButton.onClick.AddListener(() => m_stiInformationPopup.UpdateDisplay(affliction));
                infoButton.SetNameText(affliction.DisplayName);
            }

            display.UpdateLayout();
        }
    }
}