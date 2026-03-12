using System.Linq;
using HausSlytherin_SMIS;
using HausSlytherin_SMIS.Enum;
using HausSlytherin_SMIS.Enums;
using HausSlytherin_SMIS.Models;
using HausSlytherin_SMIS.Repositories;

namespace HausSlytherin_SMIS.Services;

public static class ResearcherService
{
    private static readonly ResearcherRepository _repository = RepositoryContext.Researchers;

    public static void AddResearcher()
    {
        string name;
        while (true)
        {
            Console.Write("Bitte Namen des Forschers eingeben: ");
            name = (Console.ReadLine() ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Ungültige Eingabe: Name darf nicht leer sein.");
                continue;
            }

            if (!name.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
            {
                Console.WriteLine("Fehler: Der Name darf nur Buchstaben und Leerzeichen enthalten.");
                continue;
            }

            break;
        }

        House house;
        while (true)
        {
            Console.WriteLine("Bitte Haus auswählen:");
            var values = System.Enum.GetValues<House>();
            for (var i = 0; i < values.Length; i++)
            {
                Console.WriteLine($"[{i + 1}] {values[i]}");
            }

            Console.Write("Auswahl: ");
            var input = (Console.ReadLine() ?? string.Empty).Trim();

            if (!int.TryParse(input, out var index))
            {
                Console.WriteLine("Fehler: Bitte eine gültige Zahl eingeben.");
                continue;
            }

            if (index < 1 || index > values.Length)
            {
                Console.WriteLine($"Fehler: Bitte eine Zahl zwischen 1 und {values.Length} eingeben.");
                continue;
            }

            house = values[index - 1];
            break;
        }

        string specialization;
        while (true)
        {
            Console.Write("Bitte Spezialisierung eingeben: ");
            specialization = (Console.ReadLine() ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(specialization))
            {
                Console.WriteLine("Ungültige Eingabe: Spezialisierung darf nicht leer sein.");
                continue;
            }

            break;
        }

        int experience;
        while (true)
        {
            Console.Write("Bitte Erfahrungslevel (0-100) eingeben: ");
            var input = (Console.ReadLine() ?? string.Empty).Trim();

            if (!int.TryParse(input, out experience))
            {
                Console.WriteLine("Fehler: Bitte eine gültige ganze Zahl eingeben.");
                continue;
            }

            if (experience is < 0 or > 100)
            {
                Console.WriteLine("Fehler: Erfahrungslevel muss zwischen 0 und 100 liegen.");
                continue;
            }

            break;
        }

        var researcher = new Researcher
        {
            Id = _repository.GetAll().Count + 1,
            Name = name,
            House = house,
            Specialization = specialization,
            ExperienceLevel = experience
        };

        _repository.Add(researcher);

        Logger.LogInfo(Level.Info, $"Forscher '{researcher.Name}' ({researcher.House}) hinzugefügt.", console: true);
    }

    public static void ShowAllResearchers()
    {
        var all = _repository.GetAll().OrderBy(r => r.Name).ToList();

        if (all.Count == 0)
        {
            Console.WriteLine("Keine Forscher vorhanden.");
            return;
        }

        Console.WriteLine("Registrierte Forscher:");
        foreach (var researcher in all)
        {
            researcher.PrintInfo();
        }
    }
}

