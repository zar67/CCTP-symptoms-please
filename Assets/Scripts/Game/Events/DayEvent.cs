public class DayEvent
{
    public DayEventType EventType;
}

public class NewAppointmentEvent : DayEvent
{
    public PatientData PatientData;
    public int NewAppointmentDay;
}