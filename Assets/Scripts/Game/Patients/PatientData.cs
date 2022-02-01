using System;
using System.Collections.Generic;

[Serializable]
public class PatientData
{
    public int ID;
    public string Name;
    public int PlayerStrikes;
    public string AppointmentSummary;
    public AfflictionData AfflictionData;
    public AvatarIndexData AvatarData;
    public List<int> SymptomsShown = new List<int>();
    public List<ActionType> PreviousActions = new List<ActionType>();
}