public class DayEvent
{
    public DayEventType EventType;
}

public class NewAppointmentEvent : DayEvent
{
    public int PatientID;
    public int NewAppointmentDay;
}