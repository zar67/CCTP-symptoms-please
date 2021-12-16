using UnityEngine;

public class TabletModificationsDisplay : MonoBehaviour
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

        foreach (Topic topic in ModificationsManager.ActiveTopics)
        {
            string data = m_modificationsData.GetRandomDescription(topic);

            ModificationDisplay display = Instantiate(m_displayPrefab, m_scrollViewContent);
            display.SetText(data);
        }
    }
}