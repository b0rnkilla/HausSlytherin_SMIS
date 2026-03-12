namespace HausSlytherin_SMIS.Models;

public class Creature
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Species { get; set; } = string.Empty;
    public int DangerLevel { get; set; }
    public string Habitat { get; set; } = string.Empty;
    public bool IsRestricted { get; set; }
    public string CreatureType { get; set; } = string.Empty;

    public void PrintInfo() =>
        Console.WriteLine($"ID: {Id} | {Name} | ({Species}) | Danger: {DangerLevel}");

    public bool Validate() =>
        !string.IsNullOrEmpty(Name) && DangerLevel >=1 && DangerLevel <= 10;

}