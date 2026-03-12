namespace HausSlytherin_SMIS.Models;

public class Researcher
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string House { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
    public int ExperienceLevel { get; set; }

    public void PrintInfo() =>
        Console.WriteLine($"Researcher: {Name} | House: {House} | Specialization: {Specialization} | Level: {ExperienceLevel} ");

}
