using System.Collections.Generic;
using UnityEngine;

public class SocialMediaModificationsDisplay : MonoBehaviour
{
    [SerializeField] private ModificationsData m_modificationsData = default;

    [SerializeField] private Transform m_scrollViewContent = default;
    [SerializeField] private ModificationDisplay m_displayPrefab = default;

    private void OnEnable()
    {
        foreach (Transform child in m_scrollViewContent)
        {
            Destroy(child.gameObject);
        }

        if (ModificationsManager.NumActiveTopics == 0)
        {
            ModificationsManager.ActivateTopic(Topic.TEST1, m_modificationsData.GetRandomActivateDescription(Topic.TEST1));
        }

        foreach (KeyValuePair<Topic, ModificationsManager.ModificationInstanceData> topic in ModificationsManager.ActiveTopics)
        {
            ModificationDisplay display = Instantiate(m_displayPrefab, m_scrollViewContent);
            display.SetText(topic.Value.Description);
        }
    }
}