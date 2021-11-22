using UnityEngine;

public class TabletPatientsDisplay : MonoBehaviour
{
    [SerializeField] private PatientDisplay m_displayPrefab = default;
    [SerializeField] private Transform m_scrollViewContent = default;

    private void OnEnable()
    {
        foreach (Transform child in m_scrollViewContent)
        {
            Destroy(child.gameObject);
        }

        foreach (PatientData data in PatientManager.PatientsInDay)
        {
            PatientDisplay display = Instantiate(m_displayPrefab, m_scrollViewContent);
            display.SetData(data);
        }
    }
}