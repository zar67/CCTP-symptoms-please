public class DayEvent
{
    public DayEventType EventType;
}

public class NewAppointmentEvent : DayEvent
{
    public PatientData Patient;
    public int NewAppointmentDay;
}