using SymptomsPlease.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(menuName = "SymptomsPlease/Game/PatientData")]
public class PatientData : GameScriptableObject
{
    public string PatientName => m_patientName;

    public string MainAppointmentInfo => m_mainAppointmentInfo;

    public AfflictionData AfflictionData => m_afflictionData;

    [SerializeField] private string m_patientName;
    [SerializeField] private string m_mainAppointmentInfo;
    [SerializeField] private AfflictionData m_afflictionData;
}