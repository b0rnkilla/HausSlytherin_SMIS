using HausSlytherin_SMIS.Enums;

namespace HausSlytherin_SMIS.Models;

public class Incident
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public IncidentSeverity Severity { get; set; } = IncidentSeverity.Low;
    public DateTime Date { get; set; }
    public int CreatureId { get; set; }
    
    public void PrintSummary() => Console.WriteLine($"[{Date.ToShortDateString()}] {Title}: {Description}");

}
