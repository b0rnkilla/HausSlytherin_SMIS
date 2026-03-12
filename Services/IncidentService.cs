using System.Linq;
using HausSlytherin_SMIS;
using HausSlytherin_SMIS.Enum;
using HausSlytherin_SMIS.Enums;
using HausSlytherin_SMIS.Exceptions;
using HausSlytherin_SMIS.Models;
using HausSlytherin_SMIS.Repositories;

namespace HausSlytherin_SMIS.Services;

public static class IncidentService
{
    private static readonly IncidentRepository _incidents = RepositoryContext.Incidents;
    private static readonly CreatureRepository _creatures = RepositoryContext.Creatures;

    public static void AddIncident()
    {
        if (_creatures.GetAll().Count == 0)
        {
            Console.WriteLine("Es existieren noch keine Kreaturen. Bitte zuerst eine Kreatur anlegen.");
            return;
        }

        try
        {
            Console.Write("Titel des Vorfalls: ");
            var title = (Console.ReadLine() ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new InvalidIncidentException("Der Titel des Vorfalls darf nicht leer sein.");
            }

            Console.Write("Beschreibung: ");
            var description = (Console.ReadLine() ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new InvalidIncidentException("Die Beschreibung des Vorfalls darf nicht leer sein.");
            }

            DateTime date;
            Console.Write("Datum (leer lassen für heute, Format z.B. 2025-03-15): ");
            var dateInput = (Console.ReadLine() ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(dateInput))
            {
                date = DateTime.Now;
            }
            else if (!DateTime.TryParse(dateInput, out date))
            {
                throw new InvalidIncidentException("Das angegebene Datum ist ungültig.");
            }

            IncidentSeverity severity;
            while (true)
            {
                Console.WriteLine("Bitte Schweregrad auswählen:");
                var values = System.Enum.GetValues<IncidentSeverity>();
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

                severity = values[index - 1];
                break;
            }

            Console.WriteLine("Bitte betroffene Kreatur auswählen (Id):");
            var creatures = _creatures.GetAll().OrderBy(c => c.Id).ToList();
            foreach (var c in creatures)
            {
                Console.WriteLine($"[{c.Id}] {c.Name} (Typ: {c.CreatureType}, Gefahrenlevel: {c.DangerLevel})");
            }

            Console.Write("Auswahl: ");
            var creatureInput = (Console.ReadLine() ?? string.Empty).Trim();
            if (!int.TryParse(creatureInput, out var creatureId))
            {
                throw new InvalidIncidentException("Die Kreatur-Id ist ungültig.");
            }

            var creature = _creatures.GetById(creatureId);
            if (creature == null)
            {
                throw new InvalidIncidentException($"Keine Kreatur mit Id {creatureId} gefunden.");
            }

            var incident = new Incident
            {
                Id = _incidents.GetAll().Count + 1,
                Title = title,
                Description = description,
                Date = date,
                Severity = severity,
                CreatureId = creature.Id
            };

            _incidents.Add(incident);

            Logger.LogInfo(Level.Warning, $"Neuer Vorfall '{incident.Title}' für Kreatur '{creature.Name}' (Schwere: {incident.Severity}).", console: true);
        }
        catch (InvalidIncidentException ex)
        {
            Logger.LogInfo(Level.Error, ex.Message, console: true);
        }
    }

    public static void ShowIncidents()
    {
        while (true)
        {
            Console.WriteLine("Vorfälle anzeigen:");
            Console.WriteLine("[1] Alle Vorfälle");
            Console.WriteLine("[2] Vorfälle nach Kreatur");
            Console.WriteLine("[3] Vorfälle ab Mindest-Schweregrad");
            Console.WriteLine("[0] Zurück zum Hauptmenü");
            Console.Write("Auswahl: ");

            var input = (Console.ReadLine() ?? string.Empty).Trim();
            Console.WriteLine();

            if (!int.TryParse(input, out var choice))
            {
                Console.WriteLine("Ungültige Eingabe.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    ShowAllIncidents();
                    break;
                case 2:
                    ShowIncidentsByCreature();
                    break;
                case 3:
                    ShowIncidentsBySeverity();
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Ungültige Option.");
                    break;
            }

            Console.WriteLine();
        }
    }

    private static void ShowAllIncidents()
    {
        var incidents = _incidents.GetAll().OrderByDescending(i => i.Date).ToList();
        if (incidents.Count == 0)
        {
            Console.WriteLine("Keine Vorfälle vorhanden.");
            return;
        }

        Console.WriteLine("Alle Vorfälle:");
        foreach (var incident in incidents)
        {
            var creature = _creatures.GetById(incident.CreatureId);
            Console.WriteLine($"[{incident.Date:g}] {incident.Title} (Schwere: {incident.Severity}, Kreatur: {creature?.Name ?? "unbekannt"})");
        }
    }

    private static void ShowIncidentsByCreature()
    {
        if (_creatures.GetAll().Count == 0)
        {
            Console.WriteLine("Keine Kreaturen vorhanden.");
            return;
        }

        Console.WriteLine("Bitte Kreatur (Id) auswählen:");
        var creatures = _creatures.GetAll().OrderBy(c => c.Id).ToList();
        foreach (var c in creatures)
        {
            Console.WriteLine($"[{c.Id}] {c.Name} (Gefahrenlevel: {c.DangerLevel})");
        }

        Console.Write("Auswahl: ");
        var input = (Console.ReadLine() ?? string.Empty).Trim();
        if (!int.TryParse(input, out var creatureId))
        {
            Console.WriteLine("Ungültige Eingabe.");
            return;
        }

        var incidents = _incidents.GetByCreatureId(creatureId);
        if (incidents.Count == 0)
        {
            Console.WriteLine("Keine Vorfälle für diese Kreatur vorhanden.");
            return;
        }

        Console.WriteLine($"Vorfälle für Kreatur-Id {creatureId}:");
        foreach (var incident in incidents)
        {
            Console.WriteLine($"[{incident.Date:g}] {incident.Title} (Schwere: {incident.Severity})");
        }
    }

    private static void ShowIncidentsBySeverity()
    {
        IncidentSeverity severity;
        while (true)
        {
            Console.WriteLine("Mindest-Schweregrad auswählen:");
            var values = System.Enum.GetValues<IncidentSeverity>();
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

            severity = values[index - 1];
            break;
        }

        var incidents = _incidents.GetByMinimumSeverity(severity);
        if (incidents.Count == 0)
        {
            Console.WriteLine($"Keine Vorfälle mit Schweregrad >= {severity} vorhanden.");
            return;
        }

        Console.WriteLine($"Vorfälle mit Schweregrad >= {severity}:");
        foreach (var incident in incidents)
        {
            var creature = _creatures.GetById(incident.CreatureId);
            Console.WriteLine($"[{incident.Date:g}] {incident.Title} (Schwere: {incident.Severity}, Kreatur: {creature?.Name ?? "unbekannt"})");
        }
    }
}

