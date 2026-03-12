using HausSlytherin_SMIS;
using HausSlytherin_SMIS.Enum;
using HausSlytherin_SMIS.Interfaces;
using HausSlytherin_SMIS.Models;
using HausSlytherin_SMIS.Repositories;
using HausSlytherin_SMIS.Strategies;

namespace HausSlytherin_SMIS.Services;

public class ReportService : IReportGenerator
{
    private static readonly ReportService Instance = new();

    private readonly List<RiskReport> _reports = new();

    private ReportService()
    {
    }

    public RiskReport Generate(Incident incident, Creature creature, IRiskStrategy strategy)
    {
        var report = RiskAnalysisService.Analyze(incident, creature, strategy);
        _reports.Add(report);
        Logger.LogInfo(Level.Info, $"Risikobericht für Kreatur '{creature.Name}' / Vorfall '{incident.Title}' mit Strategie '{strategy.Name}' erzeugt.", console: false);
        return report;
    }

    public static IReadOnlyList<RiskReport> GetAllReports() => Instance._reports.AsReadOnly();

    public static void PrintAllReports()
    {
        if (Instance._reports.Count == 0)
        {
            Console.WriteLine("Keine Risikoberichte vorhanden.");
            return;
        }

        Console.WriteLine("Risikoberichte:");
        foreach (var report in Instance._reports)
        {
            report.PrintReport();
            Console.WriteLine();
        }
    }

    public static void GenerateRiskReportInteractive()
    {
        var incidentRepo = RepositoryContext.Incidents;
        var creatureRepo = RepositoryContext.Creatures;

        var incidents = incidentRepo.GetAll().OrderByDescending(i => i.Date).ToList();
        if (incidents.Count == 0)
        {
            Console.WriteLine("Es existieren noch keine Vorfälle. Bitte zuerst einen Vorfall anlegen.");
            return;
        }

        var creatures = creatureRepo.GetAll().ToList();
        if (creatures.Count == 0)
        {
            Console.WriteLine("Es existieren noch keine Kreaturen. Bitte zuerst eine Kreatur anlegen.");
            return;
        }

        Console.WriteLine("Vorfall für die Risikoanalyse auswählen:");
        for (var i = 0; i < incidents.Count; i++)
        {
            Console.WriteLine($"[{i + 1}] {incidents[i].Title} ({incidents[i].Severity}, {incidents[i].Date:g})");
        }

        Console.Write("Auswahl: ");
        var incidentInput = (Console.ReadLine() ?? string.Empty).Trim();
        if (!int.TryParse(incidentInput, out var incidentIndex) || incidentIndex < 1 || incidentIndex > incidents.Count)
        {
            Console.WriteLine("Ungültige Auswahl.");
            return;
        }

        var incident = incidents[incidentIndex - 1];

        Console.WriteLine("Kreatur für die Risikoanalyse auswählen:");
        for (var i = 0; i < creatures.Count; i++)
        {
            Console.WriteLine($"[{i + 1}] {creatures[i].Name} (Gefahrenlevel: {creatures[i].DangerLevel})");
        }

        Console.Write("Auswahl: ");
        var creatureInput = (Console.ReadLine() ?? string.Empty).Trim();
        if (!int.TryParse(creatureInput, out var creatureIndex) || creatureIndex < 1 || creatureIndex > creatures.Count)
        {
            Console.WriteLine("Ungültige Auswahl.");
            return;
        }

        var creature = creatures[creatureIndex - 1];

        var strategies = new IRiskStrategy[]
        {
            new StandardRiskStrategy(),
            new StrictRiskStrategy(),
            new ResearchRiskStrategy()
        };

        Console.WriteLine("Strategie für die Risikoanalyse auswählen:");
        for (var i = 0; i < strategies.Length; i++)
        {
            Console.WriteLine($"[{i + 1}] {strategies[i].Name}");
        }

        Console.Write("Auswahl: ");
        var strategyInput = (Console.ReadLine() ?? string.Empty).Trim();
        if (!int.TryParse(strategyInput, out var strategyIndex) || strategyIndex < 1 || strategyIndex > strategies.Length)
        {
            Console.WriteLine("Ungültige Auswahl.");
            return;
        }

        var strategy = strategies[strategyIndex - 1];

        var report = Instance.Generate(incident, creature, strategy);

        Console.WriteLine();
        report.PrintReport();
    }
}
