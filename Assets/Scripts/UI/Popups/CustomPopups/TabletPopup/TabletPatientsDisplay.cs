using System.Collections.Generic;
using UnityEngine;

public class TabletPatientsDisplay : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PatientDisplay m_displayPrefab = default;
    [SerializeField] private Transform m_scrollViewContent = default;

    private List<PatientDisplay> m_patientDisplays = new List<PatientDisplay>();

    private void OnEnable()
    {
        foreach (Transform child in m_scrollViewContent)
        {
            Destroy(child.gameObject);
        }

        m_patientDisplays = new List<PatientDisplay>();
        foreach (int ID in PatientManager.PatientsInDay)
        {
            PatientData data = GameData.Patients[ID];
            PatientDisplay display = Instantiate(m_displayPrefab, m_scrollViewContent);
            display.SetData(data);

            display.SetCurrent(PatientManager.CurrentPatient == data);

            m_patientDisplays.Add(display);
        }
    }
}