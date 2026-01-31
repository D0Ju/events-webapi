namespace events_webapi.Models;

public class EventType
{
    public int Id { get; set; }
    public string Naziv { get; set; } = string.Empty;
    public string Opis { get; set; } = string.Empty;
    public int MinimalnoPolaznika { get; set; }
}
